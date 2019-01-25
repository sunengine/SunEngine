using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using SunEngine.Options;

namespace SunEngine.Services
{
    public class CryptService
    {
        public static string ToUrlSafeBase64(byte[] input)
        {
            return Convert.ToBase64String(input).Replace("+", "-").Replace("/", "_");
        }
 
        public static byte[] FromUrlSafeBase64(string input)
        {
            return Convert.FromBase64String(input.Replace("-", "+").Replace("_", "/"));
        }
        
        
        public string Crypt(string text, byte[] key, byte[] iv)
        {
            RijndaelManaged cipher = new RijndaelManaged();

            MemoryStream ms = new MemoryStream();

            CryptoStream cryptStream = new CryptoStream(ms, cipher.CreateEncryptor(key, iv),
                CryptoStreamMode.Write);

            StreamWriter sWriter = new StreamWriter(cryptStream);

            sWriter.Write(text);

            sWriter.Close();

            cryptStream.Close();

            return ToUrlSafeBase64(ms.ToArray());
        }

        public string Decrypt(string text, byte[] key, byte[] iv)
        {
            RijndaelManaged cipher = new RijndaelManaged();

            byte[] b = FromUrlSafeBase64(text);

            MemoryStream ms = new MemoryStream(b);


            CryptoStream cryptStream = new CryptoStream(ms, cipher.CreateDecryptor(key, iv),
                CryptoStreamMode.Read);

            StreamReader sReader = new StreamReader(cryptStream);

            String s = sReader.ReadToEnd();

            sReader.Close();

            return s;
        }
    }
}