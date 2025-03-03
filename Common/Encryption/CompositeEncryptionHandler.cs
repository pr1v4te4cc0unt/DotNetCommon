using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Encryption
{
    public static class CompositeEncryptionHandler
    {
        //expected format BASE64, BASIC etc
        public static string Encrypt(string val, params string[] encryptorsStringArray) 
        { 
            string result = val;
            foreach (string encryptorString in encryptorsStringArray) 
            {
                result = EncryptorFactory.GetEncryptor(encryptorString).Encrypt(result);
            }
            return result;
        }

        public static string Decrypt(string val, params string[] encryptorsStringArray) 
        {
            string result = val;
            foreach (string encryptorString in encryptorsStringArray)
            {
                result = EncryptorFactory.GetEncryptor(encryptorString).Decrypt(result);
            }
            return result;
        }

        public static string Encrypt(string val, params EncryptorType[] encryptorsArray)
        {
            string result = val;
            foreach (EncryptorType encryptor in encryptorsArray)
            {
                result = EncryptorFactory.GetEncryptor(encryptor).Encrypt(result);
            }
            return result;
        }

        public static string Decrypt(string val, params EncryptorType[] encryptorsArray)
        {
            string result = val;
            foreach (EncryptorType encryptor in encryptorsArray)
            {
                result = EncryptorFactory.GetEncryptor(encryptor).Decrypt(result);
            }
            return result;
        }
    }
}
