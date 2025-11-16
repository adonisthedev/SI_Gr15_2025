using System;
using System.Text;
using System.Security.Cryptography;

namespace CryptoLib
{
    public class AESGCMEncryptor : IEncryptor
    {
        public string Encrypt(string plaintext, byte[] key)
        {
            byte[] plainBytes = Encoding.UTF8.GetBytes(plaintext);
            byte[] nonce = SecurityHelper.GenerateRandomBytes(12);
            byte[] cipherBytes = new byte[plainBytes.Length];
            byte[] tag = new byte[16];

            using (var aesGcm = new AesGcm(key, 16))
            {
                aesGcm.Encrypt(nonce, plainBytes, cipherBytes, tag);
            }

            return $"{Convert.ToBase64String(nonce)}:{Convert.ToBase64String(cipherBytes)}:{Convert.ToBase64String(tag)}";
        }
        public string Decrypt(string cipherText, byte[] key)
        {
            var parts = cipherText.Split(':');
            if (parts.Length != 3) throw new ArgumentException("Cipher format invalid");

            byte[] nonce = Convert.FromBase64String(parts[0]);
            byte[] cipherBytes = Convert.FromBase64String(parts[1]);
            byte[] tag = Convert.FromBase64String(parts[2]);
            byte[] plainBytes = new byte[cipherBytes.Length];

            using (var aesGcm = new AesGcm(key, 16))
            {
                aesGcm.Decrypt(nonce, cipherBytes, tag, plainBytes);
            }

            return Encoding.UTF8.GetString(plainBytes);
        }

    }
}
