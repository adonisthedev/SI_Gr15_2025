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
    }
}
