using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using SunEngine.Core.Utils;

namespace SunEngine.Core.Services
{
    public class CryptService
    {
        Dictionary<string, byte[]> cryptorsKeys = new Dictionary<string, byte[]>();

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

        public void AddCryptorKey(string key)
        {
            cryptorsKeys.Add(key, GenerateSecurityKey());
        }

        public void AddCryptorKey(string key, byte[] securityKey)
        {
            cryptorsKeys.Add(key, securityKey);
        }

        public void AddCryptorKey(string key, string securityKey)
        {
            byte[] bytes = Encoding.UTF8.GetBytes(securityKey);

            if (bytes.Length < 32)
                throw new Exception("Cryptor key have to be 32 bytes length");

            if (bytes.Length > 32)
                bytes = new ArraySegment<byte>(bytes, 0, 32).ToArray();

            AddCryptorKey(key, bytes);
        }

        private static string ToUrlSafeBase64(byte[] input)
        {
            return Convert.ToBase64String(input).Replace("+", "-").Replace("/", "_");
        }

        private static byte[] FromUrlSafeBase64(string input)
        {
            return Convert.FromBase64String(input.Replace("-", "+").Replace("_", "/"));
        }


        public string Crypt(string cryptorName, string text)
        {
            var IV = GenerateIV();
            ICryptoTransform t = cipher.CreateEncryptor(cryptorsKeys[cryptorName], IV);
            byte[] textInBytes = Encoding.UTF8.GetBytes(text);
            byte[] result = t.TransformFinalBlock(textInBytes, 0, textInBytes.Length);

            byte[] resultPlusIV = new byte[result.Length + IV.Length];
            result.CopyTo(resultPlusIV, 0);
            IV.CopyTo(resultPlusIV, result.Length);

            return ToUrlSafeBase64(resultPlusIV);
        }

        public string Decrypt(string cryptorName, string text)
        {
            byte[] textInBytes = FromUrlSafeBase64(text);
            byte[] IV = new byte[16];
            Array.Copy(textInBytes, textInBytes.Length - IV.Length, IV, 0, IV.Length);

            ICryptoTransform t = cipher.CreateDecryptor(cryptorsKeys[cryptorName], IV);


            byte[] result = t.TransformFinalBlock(textInBytes, 0, textInBytes.Length - 16);

            return Encoding.UTF8.GetString(result);
        }

        private static byte[] GenerateIV()
        {
            var IV = new byte[16];
            CryptoRandomizer.CryptoProvider.GetBytes(IV);
            return IV;
        }

        private static byte[] GenerateSecurityKey()
        {
            var SequrityKey = new byte[32];
            CryptoRandomizer.CryptoProvider.GetBytes(SequrityKey);
            return SequrityKey;
        }
    }
}
