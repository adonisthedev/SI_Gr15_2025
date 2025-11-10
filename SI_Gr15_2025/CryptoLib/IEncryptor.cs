namespace CryptoLib
{
    public interface IEncryptor
    {
        string Encrypt(string plaintext, byte[] key);
    }
}
