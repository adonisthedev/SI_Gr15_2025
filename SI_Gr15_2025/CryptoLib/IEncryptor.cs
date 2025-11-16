namespace CryptoLib
{
    public interface IEncryptor
    {
        string Encrypt(string plaintext, byte[] key);
        string Decrypt(string cipherText, byte[] key);

    }
}
