using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using SunEngine.Options;

namespace SunEngine.Services
{
    public class CryptService
    {
        private readonly RijndaelManaged cipher;

        public CryptService()
        {
            cipher = new RijndaelManaged
            {
                KeySize = 256,
                BlockSize = 128,
                Padding = PaddingMode.ISO10126,
                Mode = CipherMode.CBC
            };
        }


        private static string ToUrlSafeBase64(byte[] input)
        {
            return Convert.ToBase64String(input).Replace("+", "-").Replace("/", "_");
        }

        private static byte[] FromUrlSafeBase64(string input)
        {
            return Convert.FromBase64String(input.Replace("-", "+").Replace("_", "/"));
        }
        
        
        public string Crypt(string text, byte[] key, byte[] iv)
        {
            ICryptoTransform t = cipher.CreateEncryptor(key,iv);
            byte[] textInBytes = Encoding.UTF8.GetBytes(text);
            byte[] result = t.TransformFinalBlock(textInBytes, 0, textInBytes.Length);

            return ToUrlSafeBase64(result);
        }

        public string Decrypt(string text, byte[] key, byte[] iv)
        {
            ICryptoTransform t = cipher.CreateDecryptor(key,iv);
            byte[] textInBytes = FromUrlSafeBase64(text);
            byte[] result = t.TransformFinalBlock(textInBytes, 0, textInBytes.Length);
            
            return Encoding.UTF8.GetString(result);
        }
    }
}