using System.IO;
using System.Security.Cryptography;
using System.Text;
using Core.Logging;
using Data;
using UnityEngine;

namespace Managers
{
    /// <summary>
    /// 게임 데이터의 저장 및 로드를 담당하는 클래스입니다.
    /// </summary>
    public class SaveLoadManager
    {
        private readonly string _savePath;
        private readonly string _encryptionKey = "ThisIsASecretKeyForEncryption123"; // TODO: 실제 프로젝트에서는 더 안전한 키 관리 방법 사용

        public SaveLoadManager()
        {
            _savePath = Path.Combine(Application.persistentDataPath, "playerdata.json");
        }

        /// <summary>
        /// PlayerData를 파일에 저장합니다.
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
        /// 파일에서 PlayerData를 불러옵니다.
        /// </summary>
        /// <returns>불러온 플레이어 데이터. 파일이 없으면 null을 반환합니다.</returns>
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
                return null;
            }
        }
        
        // --- 간단한 AES 암호화/복호화 ---
        private string Encrypt(string plainText)
        {
            byte[] iv = new byte[16];
            byte[] array;

            using (Aes aes = Aes.Create())
            {
                aes.Key = Encoding.UTF8.GetBytes(_encryptionKey);
                aes.IV = iv;

                ICryptoTransform encryptor = aes.CreateEncryptor(aes.Key, aes.IV);

                using (MemoryStream memoryStream = new MemoryStream())
                {
                    using (CryptoStream cryptoStream = new CryptoStream(memoryStream, encryptor, CryptoStreamMode.Write))
                    {
                        using (StreamWriter streamWriter = new StreamWriter(cryptoStream))
                        {
                            streamWriter.Write(plainText);
                        }
                        array = memoryStream.ToArray();
                    }
                }
            }
            return System.Convert.ToBase64String(array);
        }

        private string Decrypt(string cipherText)
        {
            byte[] iv = new byte[16];
            byte[] buffer = System.Convert.FromBase64String(cipherText);

            using (Aes aes = Aes.Create())
            {
                aes.Key = Encoding.UTF8.GetBytes(_encryptionKey);
                aes.IV = iv;
                ICryptoTransform decryptor = aes.CreateDecryptor(aes.Key, aes.IV);

                using (MemoryStream memoryStream = new MemoryStream(buffer))
                {
                    using (CryptoStream cryptoStream = new CryptoStream(memoryStream, decryptor, CryptoStreamMode.Read))
                    {
                        using (StreamReader streamReader = new StreamReader(cryptoStream))
                        {
                            return streamReader.ReadToEnd();
                        }
                    }
                }
            }
        }
    }
}
