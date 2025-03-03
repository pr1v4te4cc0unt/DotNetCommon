namespace Common.Encryption
{
    public interface IEncryptor
    {
        string Encrypt(string val);

        string Decrypt(string val);
    }
}
