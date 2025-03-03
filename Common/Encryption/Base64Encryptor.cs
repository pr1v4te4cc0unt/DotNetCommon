using System.Text;
using System.Text.RegularExpressions;

namespace Common.Encryption
{
    public class Base64Encryptor : IEncryptor
    {
        #region ctor
        public Base64Encryptor() { }
        #endregion

        #region public
        //btoa encoding
        public string Encrypt(string val)
        {
            return Convert.ToBase64String(Encoding.UTF8.GetBytes(val));
        }

        //atob decoding
        public string Decrypt(string val)
        {
            if (!IsEncrypted(val))
            {
                return val;
            }
            return Encoding.UTF8.GetString(Convert.FromBase64String(val));
        }
        #endregion

        #region private
        private bool IsEncrypted(string val)
        {
            return Regex.IsMatch(val, RegexPattern.Base64Encryption);
        }
        #endregion
    }
}
