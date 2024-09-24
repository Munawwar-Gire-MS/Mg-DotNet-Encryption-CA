using System;
using System.Security.Cryptography;
using System.Text;

namespace MyEncryptionApp.Services // Ensure this matches your project namespace
{
    public class EncryptionService
    {
        private readonly byte[] _encryptionKey;

        public EncryptionService(string primaryEncryptionKey)
        {
            if (primaryEncryptionKey.Length != 16 && primaryEncryptionKey.Length != 24 && primaryEncryptionKey.Length != 32)
            {
                throw new ArgumentException("The encryption key must be 16, 24, or 32 characters long.");
            }

            _encryptionKey = Encoding.UTF8.GetBytes(primaryEncryptionKey);
        }

        public string Encrypt(string plainText)
        {
            using (var aes = Aes.Create())
            {
                aes.Key = _encryptionKey;
                aes.GenerateIV();
                var iv = aes.IV;

                using (var encryptor = aes.CreateEncryptor(aes.Key, iv))
                {
                    var plainBytes = Encoding.UTF8.GetBytes(plainText);
                    var encryptedBytes = encryptor.TransformFinalBlock(plainBytes, 0, plainBytes.Length);

                    var result = new byte[iv.Length + encryptedBytes.Length];
                    Buffer.BlockCopy(iv, 0, result, 0, iv.Length);
                    Buffer.BlockCopy(encryptedBytes, 0, result, iv.Length, encryptedBytes.Length);

                    return Convert.ToBase64String(result);
                }
            }
        }

        public string Decrypt(string encryptedText)
        {
            var fullCipher = Convert.FromBase64String(encryptedText);

            using (var aes = Aes.Create())
            {
                aes.Key = _encryptionKey;

                var iv = new byte[aes.BlockSize / 8];
                var cipherBytes = new byte[fullCipher.Length - iv.Length];

                Buffer.BlockCopy(fullCipher, 0, iv, 0, iv.Length);
                Buffer.BlockCopy(fullCipher, iv.Length, cipherBytes, 0, cipherBytes.Length);

                aes.IV = iv;

                using (var decryptor = aes.CreateDecryptor(aes.Key, aes.IV))
                {
                    var decryptedBytes = decryptor.TransformFinalBlock(cipherBytes, 0, cipherBytes.Length);
                    return Encoding.UTF8.GetString(decryptedBytes);
                }
            }
        }
    }
}
