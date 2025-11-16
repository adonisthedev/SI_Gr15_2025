using System;
using CryptoLib;

class Program
{
    static void Main()
    {
        string userPassword = "UltraSecurePassword!";
        string secretMessage = "Ky eshte nje mesazh sekret.";

        Console.WriteLine("- Fillimi i programit -\n");

        string hash = UserPasswordManager.GeneratePasswordHash(userPassword);
        bool valid = UserPasswordManager.ValidatePassword(userPassword, hash);

        Console.WriteLine("- Verifikimi i fjalekalimit -");
        var hashParts = hash.Split(':');

        Console.WriteLine($"Algoritmi : {hashParts[0]}");
        Console.WriteLine($"Iteracionet: {hashParts[1]}");
        Console.WriteLine($"Salt       : {hashParts[2]}");
        Console.WriteLine($"Hash       : {hashParts[3]}");
        Console.WriteLine($"Fjalekalimi valid: {valid}\n");

        byte[] salt = Convert.FromBase64String(hash.Split(':')[2]);
        byte[] key = UserKeyManager.GenerateKeyFromPassword(userPassword, salt);

        IEncryptor encryptor = new AESGCMEncryptor();
        string cipher = encryptor.Encrypt(secretMessage, key);

        Console.WriteLine("- Enkriptimi me AES-GCM (komponentet) -");
        var cipherParts = cipher.Split(':');
        Console.WriteLine($"Nonce      : {cipherParts[0]}");
        Console.WriteLine($"Ciphertext : {cipherParts[1]}");
        Console.WriteLine($"Tag        : {cipherParts[2]}\n");

        string recovered = encryptor.Decrypt(cipher, key);

        Console.WriteLine("- Dekriptimi me AES-GCM -");
        Console.WriteLine($"Mesazhi i rikuperuar: {recovered}\n");

        Console.WriteLine("- Perfundimi i programit -");
    }
}
