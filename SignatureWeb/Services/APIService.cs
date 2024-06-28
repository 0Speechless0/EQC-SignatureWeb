using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;

namespace EQC.Services
{
    public class APIService
    {

        public static Dictionary<string, Token> tokens { get; set; } = new Dictionary<string, Token>();


        public class Token
        {
            public byte[] pwdHashing { get; set; }
            public byte[] IVHashing { get; set; }
            public byte[] cipherText { get; set; }
            public string userNo { get; set; }
            public string Value { get; set; }
            public DateTime CreateTime { get; set; }
        }
        public bool checkTokenVaild(string token)
        {

            if (token == null)
            {
                return false;
            }
            //token是否存在
            Token myToken =
                tokens.Select(t => t.Value)
                .Where(t => t.Value == token).FirstOrDefault();
            if (myToken == null)
            {
                return false;
            }
            string assertUserNo = DecryptStringFromBytes_Aes(
                myToken.cipherText, myToken.pwdHashing, myToken.IVHashing);


            //檢查是否在有效時間內
            if (myToken.CreateTime < DateTime.UtcNow.AddHours(-1))
            {

                return false;
            }

            //驗證失敗
            if (assertUserNo != myToken.userNo) return false;

            return true;

        }


        internal void addToken(Token token)
        {
            if (tokens.ContainsKey(token.userNo))
            {

                tokens.Remove(token.userNo);
            }

            tokens.Add(token.userNo, token);

           
        }
        internal string addToken(string userNo, string pwd)
        {
            byte[] pwdKey = pwd.ToCharArray()
                .Select(c => (byte)c)
                .ToArray();
            byte[] IV = DateTime.Now.ToLongTimeString()
                .Select(c => (byte)c)
                .ToArray();
            if (pwdKey == null) throw new Exception("該帳號未註冊");
            var pwdHashing = new MD5CryptoServiceProvider().ComputeHash(pwdKey);
            var IVHashing = new MD5CryptoServiceProvider().ComputeHash(IV);
            StringBuilder tokenBuilder = new StringBuilder();

            byte[] cipherText = EncryptStringToBytes_Aes(userNo, pwdHashing, IVHashing);

            cipherText.ToList()
                .ForEach(b => tokenBuilder.Append(b.ToString("x2")));
            string token = tokenBuilder.ToString();
            if (tokens.ContainsKey(userNo))
            {

                tokens.Remove(userNo);
            }

            tokens.Add(userNo, new Token
            {
                cipherText = cipherText,
                pwdHashing = pwdHashing,
                IVHashing = IVHashing,
                userNo = userNo,
                Value = token,
                CreateTime = DateTime.Now
            });
            return token;
        }

        internal int removeToken(string token)
        {
            string userNo = tokens.ToList().Find(item => item.Value.Value == token).Key;
            if (userNo == null)
            {
                return 0;
            }
            if (tokens.Remove(userNo))
            {
                return 1;
            }

            return -1;

        }
        static byte[] EncryptStringToBytes_Aes(string plainText, byte[] Key, byte[] IV)
        {
            // Check arguments.
            if (plainText == null || plainText.Length <= 0)
                throw new ArgumentNullException("plainText");
            if (Key == null || Key.Length <= 0)
                throw new ArgumentNullException("Key");
            if (IV == null || IV.Length <= 0)
                throw new ArgumentNullException("IV");
            byte[] encrypted;

            // Create an Aes object
            // with the specified key and IV.
            using (Aes aesAlg = Aes.Create())
            {
                aesAlg.Key = Key;
                aesAlg.IV = IV;

                // Create an encryptor to perform the stream transform.
                ICryptoTransform encryptor = aesAlg.CreateEncryptor(aesAlg.Key, aesAlg.IV);

                // Create the streams used for encryption.
                using (MemoryStream msEncrypt = new MemoryStream())
                {
                    using (CryptoStream csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write))
                    {
                        using (StreamWriter swEncrypt = new StreamWriter(csEncrypt))
                        {
                            //Write all data to the stream.
                            swEncrypt.Write(plainText);
                        }
                        encrypted = msEncrypt.ToArray();
                    }
                }
            }

            // Return the encrypted bytes from the memory stream.
            return encrypted;
        }

        static string DecryptStringFromBytes_Aes(byte[] cipherText, byte[] Key, byte[] IV)
        {
            // Check arguments.
            if (cipherText == null || cipherText.Length <= 0)
                throw new ArgumentNullException("cipherText");
            if (Key == null || Key.Length <= 0)
                throw new ArgumentNullException("Key");
            if (IV == null || IV.Length <= 0)
                throw new ArgumentNullException("IV");

            // Declare the string used to hold
            // the decrypted text.
            string plaintext = null;

            // Create an Aes object
            // with the specified key and IV.
            using (Aes aesAlg = Aes.Create())
            {
                aesAlg.Key = Key;
                aesAlg.IV = IV;

                // Create a decryptor to perform the stream transform.
                ICryptoTransform decryptor = aesAlg.CreateDecryptor(aesAlg.Key, aesAlg.IV);

                // Create the streams used for decryption.
                using (MemoryStream msDecrypt = new MemoryStream(cipherText))
                {
                    using (CryptoStream csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read))
                    {
                        using (StreamReader srDecrypt = new StreamReader(csDecrypt))
                        {

                            // Read the decrypted bytes from the decrypting stream
                            // and place them in a string.
                            plaintext = srDecrypt.ReadToEnd();
                        }
                    }
                }
            }

            return plaintext;
        }
    }
}