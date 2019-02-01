using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using SunEngine.Commons.Utils;

namespace SunEngine.Services
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
            cryptorsKeys.Add(key, GenerateSequrityKey());
        }

        public void AddCryptorKey(string key, byte[] sequrityKey)
        {
            cryptorsKeys.Add(key, sequrityKey);
        }

        /*public void AddCryptor(string key, string seurityKey, string vector)
        {
            var numberOfBits = 256;
            byte[] SecurityKey = new byte[numberOfBits / 8]; 
            
            
            CryptoRandomizer.CryptoProvider.GetBytes(SecurityKey);
            
            numberOfBits = 128;
            byte[] IV = new byte[numberOfBits / 8];
            CryptoRandomizer.CryptoProvider.GetBytes(IV);
            
            
            Cryptors.Add(key,
                new Cryptor(cipher.CreateEncryptor(SecurityKey,IV),
                    cipher.CreateDecryptor(SecurityKey,IV)));
        }*/


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

            var z = ToUrlSafeBase64(resultPlusIV);

            return z;
        }

        public string Decrypt(string cryptorName, string text)
        {
            byte[] textInBytes = FromUrlSafeBase64(text);
            byte[] IV = new byte[16];
            Array.Copy(textInBytes,textInBytes.Length - IV.Length,IV,0,IV.Length);
            
            ICryptoTransform t = cipher.CreateDecryptor(cryptorsKeys[cryptorName],IV);

            
            byte[] result = t.TransformFinalBlock(textInBytes, 0, textInBytes.Length - 16);

            var z = Encoding.UTF8.GetString(result);
            return z;
        }

        private byte[] GenerateIV()
        {
            var IV = new byte[16];
            CryptoRandomizer.CryptoProvider.GetBytes(IV);
            return IV;
        }
        
        private byte[] GenerateSequrityKey()
        {
            var SequrityKey = new byte[32];
            CryptoRandomizer.CryptoProvider.GetBytes(SequrityKey);
            return SequrityKey;
        }
    }
}