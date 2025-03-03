using Common.EnumUtil;

namespace Common.Encryption
{
    public enum EncryptorType { BASIC, BASE64, BASE64_BASIC }

    public static class EncryptorFactory
    {
        #region
        public static IEncryptor GetEncryptor(EncryptorType encryptorType)
        {
            IEncryptor encryptor = null;

            switch (encryptorType)
            {
                case EncryptorType.BASIC:
                    encryptor = new Encryptor();
                    break;
                case EncryptorType.BASE64:
                    encryptor = new Base64Encryptor();
                    break;
                default:
                    throw new NotImplementedException();
            }
            return encryptor;
        }

        public static IEncryptor GetEncryptor(string encryptorTypeString)
        {
            EncryptorType encryptorType = EnumHelper.TryParse<EncryptorType>(encryptorTypeString);
            return GetEncryptor(encryptorType);
        }
        #endregion
    }
}
