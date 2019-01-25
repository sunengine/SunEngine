using System;
using System.Security.Cryptography;
using System.Text;
using SunEngine.Options;

namespace SunEngine.Services
{
    public class CryptService
    {

        public string Crypt(string text, byte[] key, byte[] iv)
        {
            RijndaelManaged cipher = new RijndaelManaged
            {
                KeySize = 256,
                BlockSize = 128,
                Padding = PaddingMode.ISO10126,
                Mode = CipherMode.CBC,
            };

            ICryptoTransform t = cipher.CreateEncryptor(key,iv);
            byte[] textInBytes = Encoding.UTF8.GetBytes(text);
            byte[] result = t.TransformFinalBlock(textInBytes, 0, textInBytes.Length);

            return Convert.ToBase64String(result);
        }

        public string Decrypt(string text, byte[] key, byte[] iv)
        {
            RijndaelManaged cipher = new RijndaelManaged
            {
                KeySize = 256,
                BlockSize = 128,
                Padding = PaddingMode.ISO10126,
                Mode = CipherMode.CBC,
            };
            
            ICryptoTransform t = cipher.CreateDecryptor(key,iv);
            byte[] textInBytes = Convert.FromBase64String(text);
            byte[] result = t.TransformFinalBlock(textInBytes, 0, textInBytes.Length);
            var rez = Encoding.UTF8.GetString(result);
            return rez;
        }
    }
}