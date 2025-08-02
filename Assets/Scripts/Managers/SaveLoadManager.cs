using System.IO;
using System.Security.Cryptography;
using System.Text;
using Core.Logging;
using Data;
using Managers.Interfaces;
using UnityEngine;

namespace Managers
{
    /// <summary>
    /// 게임 데이터의 저장 및 로드를 담당하는 클래스입니다.
    /// 데이터는 AES 암호화를 거쳐 파일에 저장됩니다.
    /// </summary>
    public class SaveLoadManager : ISaveLoadManager
    {
        private readonly string _savePath;
        private readonly byte[] _encryptionKey;

        private const string Salt = "DungeonMasterSaltString"; // 키 유도를 위한 고정 솔트 값

        public SaveLoadManager()
        {
            _savePath = Path.Combine(Application.persistentDataPath, "playerdata.sav"); // 확장자 변경
            
            // 기기 고유 ID와 솔트를 조합하여 암호화 키를 동적으로 생성합니다.
            // 이렇게 하면 기기마다 다른 키를 사용하게 되어 보안이 강화됩니다.
            string combinedKey = SystemInfo.deviceUniqueIdentifier + Salt;
            
            // PBKDF2 알고리즘을 사용하여 해시 기반으로 키를 파생시킵니다.
            // 32바이트(256비트) 키를 생성합니다.
            using (var rfc2898 = new Rfc2898DeriveBytes(combinedKey, Encoding.UTF8.GetBytes(Salt), 10000, HashAlgorithmName.SHA256))
            {
                _encryptionKey = rfc2898.GetBytes(32); 
            }
        }

        public void SaveData()
        {
            // 이 메서드는 외부(GameFlowManager)에서 PlayerData를 받아 처리하도록 변경될 것입니다.
            // 현재는 인터페이스와의 호환성을 위해 시그니처만 유지합니다.
            throw new System.NotImplementedException("SaveData(PlayerData)를 사용해주세요.");
        }

        /// <summary>
        /// PlayerData를 암호화하여 파일에 저장합니다.
        /// </summary>
        /// <param name="playerData">저장할 플레이어 데이터</param>
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

        /// <summary>
        /// 파일에서 PlayerData를 복호화하여 불러옵니다.
        /// </summary>
        /// <returns>불러온 플레이어 데이터. 파일이 없거나 오류 발생 시 null을 반환합니다.</returns>
        public PlayerData LoadData()
        {
            if (!File.Exists(_savePath))
            {
                GameLogger.LogWarning("저장된 데이터 파일이 없습니다.");
                return null;
            }

            try
            {
                string encryptedJson = File.ReadAllText(_savePath);
                string json = Decrypt(encryptedJson);
                PlayerData playerData = JsonUtility.FromJson<PlayerData>(json);
                GameLogger.Log("데이터 로드 완료.");
                return playerData;
            }
            catch (System.Exception e)
            {
                GameLogger.LogError("데이터 로드 중 오류 발생. 새 데이터를 생성합니다.", e);
                File.Delete(_savePath); // 손상된 파일일 수 있으므로 삭제
                return null;
            }
        }

        void ISaveLoadManager.LoadData()
        {
             // 이 메서드는 외부(GameFlowManager)에서 PlayerData를 받아 처리하도록 변경될 것입니다.
             // 현재는 인터페이스와의 호환성을 위해 시그니처만 유지합니다.
            throw new System.NotImplementedException("LoadData()는 PlayerData를 반환합니다.");
        }
        
        // --- AES-256-CBC 암호화/복호화 ---
        private string Encrypt(string plainText)
        {
            using (Aes aes = Aes.Create())
            {
                aes.Key = _encryptionKey;
                aes.GenerateIV(); // 매번 새로운 IV(초기화 벡터)를 생성하여 보안 강화
                var iv = aes.IV;

                using (var encryptor = aes.CreateEncryptor(aes.Key, iv))
                using (var memoryStream = new MemoryStream())
                {
                    // IV를 암호화된 데이터 앞에 붙여서 저장합니다.
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
                
                // 데이터 앞부분에서 IV를 다시 읽어옵니다.
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
