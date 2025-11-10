using System;
using System.Security.Cryptography;

namespace CryptoLib
{
    public static class SecurityHelper
    {
        public static byte[] GenerateRandomBytes(int length)
        {
            byte[] bytes = new byte[length];
            using var rng = RandomNumberGenerator.Create();
            rng.GetBytes(bytes);
            return bytes;
        }

        public static bool FixedTimeEquals(byte[] a, byte[] b)
        {
            return CryptographicOperations.FixedTimeEquals(a, b);
        }
    }
}
