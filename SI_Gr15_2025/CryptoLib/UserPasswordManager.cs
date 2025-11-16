using System;
using System.Security.Cryptography;

namespace CryptoLib
{
    public static class UserPasswordManager
    {
        private const int SaltLength = 20;
        private const int HashLength = 64;
        private const int Iterations = 250_000;

        public static string GeneratePasswordHash(string password)
        {
            byte[] salt = SecurityHelper.GenerateRandomBytes(SaltLength);
            using var pbkdf2 = new Rfc2898DeriveBytes(password, salt, Iterations, HashAlgorithmName.SHA512);
            byte[] hash = pbkdf2.GetBytes(HashLength);
            return $"PBKDF2-SHA512:{Iterations}:{Convert.ToBase64String(salt)}:{Convert.ToBase64String(hash)}";
        }

        public static bool ValidatePassword(string password, string storedHash)
        {
            var parts = storedHash.Split(':');
            if (parts.Length != 4 || parts[0] != "PBKDF2-SHA512") return false;

            int iter = int.Parse(parts[1]);
            byte[] salt = Convert.FromBase64String(parts[2]);
            byte[] storedBytes = Convert.FromBase64String(parts[3]);

            using var pbkdf2 = new Rfc2898DeriveBytes(password, salt, iter, HashAlgorithmName.SHA512);
            byte[] computed = pbkdf2.GetBytes(storedBytes.Length);

            return SecurityHelper.FixedTimeEquals(computed, storedBytes);
        }
    }
}
