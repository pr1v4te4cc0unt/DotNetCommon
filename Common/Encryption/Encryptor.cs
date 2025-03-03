using System.Text;
using System.Text.RegularExpressions;

namespace Common.Encryption
{
    public class Encryptor : IEncryptor
    {
        #region prop
        private string Key = "ADMIN";
        #endregion

        #region ctor
        public Encryptor() { }

        public Encryptor(string key) 
        { 
            Key = key;
        }
        #endregion

        #region public
        public string Encrypt(string val)
        {
            string[] result = new string[val.Length];
            for (int i = 0, j = 0; j < val.Length; i++, j++)
            {
                if (i == Key.Length)
                {
                    i = 0;
                }
                int keyCharCode = Encoding.UTF8.GetBytes(Key[i].ToString())[0];
                int valCharCode = Encoding.UTF8.GetBytes(val[j].ToString())[0];
                int encrypted = (valCharCode * keyCharCode);
                result[j] = encrypted.ToString();
            }
            return string.Join(',', result);
        }

        public string Decrypt(string val)
        {
            if (!IsEncrypted(val))
            {
                return val;
            }

            byte[] result = new byte[val.Length];
            string[] valArray = val.Split(',');
            for (int i = 0, j = 0; j < valArray.Length; i++, j++)
            {
                if (i == Key.Length)
                {
                    i = 0;
                }
                int keyCharCode = Encoding.UTF8.GetBytes(Key[i].ToString())[0];
                int valCharCode = int.Parse(valArray[j]);
                int decrypted = (valCharCode / keyCharCode);
                result[j] = (byte)decrypted;
            }
            return Encoding.UTF8.GetString(result);
        }
        #endregion

        #region private
        private bool IsEncrypted(string val)
        {
            return Regex.IsMatch(val, RegexPattern.BaseEncryption);
        }
        #endregion
    }
}
