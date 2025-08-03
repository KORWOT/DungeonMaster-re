using System.IO;
using System.Security.Cryptography;
using System.Text;
using DungeonMaster.Core.Logging;
using DungeonMaster.Data;
using DungeonMaster.Core.Interfaces;
using UnityEngine;

namespace DungeonMaster.Core
{
    /// <summary>
    /// 게임 데이터의 저장 및 로드를 담당하는 클래스입니다.
    /// 데이터는 AES 암호화를 거쳐 파일에 저장됩니다.
    /// </summary>
    public class SaveLoadManager : ISaveLoadManager
    {
        public PlayerData LoadedData { get; private set; }
        
        private readonly string _savePath;
        private readonly byte[] _encryptionKey;

        private const string Salt = "DungeonMasterSaltString"; // 키 유도를 위한 고정 솔트 값

        public SaveLoadManager()
        {
            _savePath = Path.Combine(Application.persistentDataPath, "playerdata.sav");
            
            string combinedKey = SystemInfo.deviceUniqueIdentifier + Salt;
            
            using (var rfc2898 = new Rfc2898DeriveBytes(combinedKey, Encoding.UTF8.GetBytes(Salt), 10000, HashAlgorithmName.SHA256))
            {
                _encryptionKey = rfc2898.GetBytes(32); 
            }
        }

        public void SaveData(PlayerData playerData)
        {
            try
            {
                string json = JsonUtility.ToJson(playerData, true);
                string encryptedJson = Encrypt(json);
                File.WriteAllText(_savePath, encryptedJson);
                GameLogger.Log($"데이터 저장 완료: {_savePath}");
            }
            catch (System.Exception e)
            {
                GameLogger.LogError("데이터 저장 중 오류 발생", e);
            }
        }

        public void LoadData()
        {
            if (!File.Exists(_savePath))
            {
                GameLogger.LogWarning("저장된 데이터 파일이 없습니다.");
                LoadedData = new PlayerData(); // 새 데이터 생성
                return;
            }

            try
            {
                string encryptedJson = File.ReadAllText(_savePath);
                string json = Decrypt(encryptedJson);
                LoadedData = JsonUtility.FromJson<PlayerData>(json);
                if (LoadedData == null)
                {
                    LoadedData = new PlayerData();
                    GameLogger.LogWarning("로드된 데이터가 null입니다. 새 데이터를 생성합니다.");
                }
                GameLogger.Log("데이터 로드 완료.");
            }
            catch (System.Exception e)
            {
                GameLogger.LogError("데이터 로드 중 오류 발생. 새 데이터를 생성합니다.", e);
                File.Delete(_savePath); // 손상된 파일일 수 있으므로 삭제
                LoadedData = new PlayerData();
            }
        }
        
        // --- AES-256-CBC 암호화/복호화 ---
        private string Encrypt(string plainText)
        {
            using (Aes aes = Aes.Create())
            {
                aes.Key = _encryptionKey;
                aes.GenerateIV();
                var iv = aes.IV;

                using (var encryptor = aes.CreateEncryptor(aes.Key, iv))
                using (var memoryStream = new MemoryStream())
                {
                    memoryStream.Write(iv, 0, iv.Length);
                    using (var cryptoStream = new CryptoStream(memoryStream, encryptor, CryptoStreamMode.Write))
                    using (var streamWriter = new StreamWriter(cryptoStream))
                    {
                        streamWriter.Write(plainText);
                    }
                    return System.Convert.ToBase64String(memoryStream.ToArray());
                }
            }
        }

        private string Decrypt(string cipherText)
        {
            var fullCipher = System.Convert.FromBase64String(cipherText);

            using (Aes aes = Aes.Create())
            {
                aes.Key = _encryptionKey;
                
                var iv = new byte[aes.BlockSize / 8];
                System.Buffer.BlockCopy(fullCipher, 0, iv, 0, iv.Length);
                aes.IV = iv;

                using (var decryptor = aes.CreateDecryptor(aes.Key, aes.IV))
                using (var memoryStream = new MemoryStream(fullCipher, iv.Length, fullCipher.Length - iv.Length))
                using (var cryptoStream = new CryptoStream(memoryStream, decryptor, CryptoStreamMode.Read))
                using (var streamReader = new StreamReader(cryptoStream))
                {
                    return streamReader.ReadToEnd();
                }
            }
        }
    }
}
