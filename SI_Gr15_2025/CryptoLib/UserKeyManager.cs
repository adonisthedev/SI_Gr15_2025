using System;
using System.Security.Cryptography;

namespace CryptoLib
{
    public static class UserKeyManager
    {
        public static byte[] GenerateKeyFromPassword(string password, byte[] salt, int keyBytes = 32)
        {
            using var pbkdf2 = new Rfc2898DeriveBytes(password, salt, 250_000, HashAlgorithmName.SHA512);
            return pbkdf2.GetBytes(keyBytes);
        }
    }
}
