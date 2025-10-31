using System;
using System.Security.Cryptography;
using System.Text;

class Program
{
    private const int SaltSize = 16;           // 16 bytes = 128 bits
    private const int KeySize = 32;            // 32 bytes = 256 bits (per AES-256)
    private const int HashSize = 64;           // 64 bytes = 512 bits (per ruajtjen e hash password)
    private const int Iterations = 200_000;    // Numer iteracionesh (rregullo sipas kapacitetit te serverit)

    static void Main()
    {
        Console.WriteLine("Shembull PBKDF2-SHA512 + AES-GCM\n");

        string password = "S3kritP@ssw0rd!";
        Console.WriteLine($"Password origjinal: {password}");

        // Hash password per ruajtje
        string stored = HashPassword(password);
        Console.WriteLine($"Stored hash: {stored}");

        // Verifikim
        bool ok = VerifyPassword(password, stored);
        Console.WriteLine($"Verifikim i sakte: {ok}");

        // Enkripto / Dekripto nje tekst me celes te nxjerre nga fjalekalimi
        string plaintext = "Kjo eshte nje mesazh i fshehte.";
        Console.WriteLine($"\nPlaintext: {plaintext}");

        // Derivo celes nga password (mund ta perdorim per enkriptim)
        byte[] salt = Convert.FromBase64String(stored.Split(':')[1]);
        byte[] key = DeriveKey(Encoding.UTF8.GetBytes(password), salt, Iterations, KeySize);

        // Enkripto
        var cipher = EncryptString(plaintext, key);
        Console.WriteLine($"Cipher (base64): {cipher}");

        // Dekripto
        string recovered = DecryptString(cipher, key);
        Console.WriteLine($"Recovered plaintext: {recovered}");
    }

    // Hash password me PBKDF2-SHA512 dhe ruaj si: iterations:base64(salt):base64(hash)
    public static string HashPassword(string password)
    {
        // Gjenero salt
        byte[] salt = new byte[SaltSize];
        using (var rng = RandomNumberGenerator.Create())
        {
            rng.GetBytes(salt);
        }

        // Nxirr hash me PBKDF2-SHA512
        byte[] hash;
        using (var pbkdf2 = new Rfc2898DeriveBytes(password, salt, Iterations, HashAlgorithmName.SHA512))
        {
            hash = pbkdf2.GetBytes(HashSize);
        }

        // Ruaj formatin: iterations:saltBase64:hashBase64
        string result = $"{Iterations}:{Convert.ToBase64String(salt)}:{Convert.ToBase64String(hash)}";
        return result;
    }

    // ----------------------------
    // Verifikon password kundrejt vleres se ruajtur
    // ----------------------------
    public static bool VerifyPassword(string password, string stored)
    {
        try
        {
            var parts = stored.Split(':');
            if (parts.Length != 3) return false;

            int iterations = int.Parse(parts[0]);
            byte[] salt = Convert.FromBase64String(parts[1]);
            byte[] storedHash = Convert.FromBase64String(parts[2]);

            using (var pbkdf2 = new Rfc2898DeriveBytes(password, salt, iterations, HashAlgorithmName.SHA512))
            {
                byte[] computed = pbkdf2.GetBytes(storedHash.Length);
                // Secure comparison (time-constant)
                return CryptographicOperations.FixedTimeEquals(computed, storedHash);
            }
        }
        catch
        {
            return false;
        }
    }

    // ----------------------------
    // Derivon nje çeles binar (p.sh. 256-bit) nga password + salt
    // ----------------------------
    public static byte[] DeriveKey(byte[] passwordBytes, byte[] salt, int iterations, int outputBytes)
    {
        using (var pbkdf2 = new Rfc2898DeriveBytes(passwordBytes, salt, iterations, HashAlgorithmName.SHA512))
        {
            return pbkdf2.GetBytes(outputBytes);
        }
    }

    // ----------------------------
    // Encrypt string me AES-GCM (ky perdor nje çeles 256-bit)
    // Kthen nje string base64 qe permban: base64(nonce)|base64(ciphertext)|base64(tag)
    // ----------------------------
    public static string EncryptString(string plaintext, byte[] key)
    {
        byte[] plaintextBytes = Encoding.UTF8.GetBytes(plaintext);
        byte[] nonce = new byte[12]; // 96-bit nonce rekomandohet per GCM
        using (var rng = RandomNumberGenerator.Create())
        {
            rng.GetBytes(nonce);
        }

        byte[] ciphertext = new byte[plaintextBytes.Length];
        byte[] tag = new byte[16]; // 128-bit tag

        using (var aesGcm = new AesGcm(key))
        {
            aesGcm.Encrypt(nonce, plaintextBytes, ciphertext, tag, null);
        }

        // Paketoj dhe kthe si string i sigurt (format i thjeshte, por mund ta ndryshosh)
        string outStr = Convert.ToBase64String(nonce) + ":" +
                        Convert.ToBase64String(ciphertext) + ":" +
                        Convert.ToBase64String(tag);

        return outStr;
    }

    // ----------------------------
    // Decrypt nga formati base64(nonce):base64(cipher):base64(tag)
    // ----------------------------
    public static string DecryptString(string combined, byte[] key)
    {
        var parts = combined.Split(':');
        if (parts.Length != 3) throw new ArgumentException("Format i gabuar i cipher text");

        byte[] nonce = Convert.FromBase64String(parts[0]);
        byte[] ciphertext = Convert.FromBase64String(parts[1]);
        byte[] tag = Convert.FromBase64String(parts[2]);

        byte[] plaintext = new byte[ciphertext.Length];

        using (var aesGcm = new AesGcm(key))
        {
            aesGcm.Decrypt(nonce, ciphertext, tag, plaintext, null);
        }

        return Encoding.UTF8.GetString(plaintext);
    }
}
