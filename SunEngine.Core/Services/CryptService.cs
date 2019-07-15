using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using LinqToDB;
using SunEngine.Core.Cache.Services;
using SunEngine.Core.DataBase;
using SunEngine.Core.Errors;
using SunEngine.Core.Models;
using SunEngine.Core.Utils;

namespace SunEngine.Core.Services
{
    public interface ICryptService : ISunMemoryCache
    {
        string Crypt(string cipherName, string text);
        string Decrypt(string cipherName, string text);
        void AddCipherKey(string key);
        Task ResetSecret(string name);
        Task ResetAllSecrets();
    }

    public class CryptService : ICryptService
    {
        private readonly Dictionary<string, byte[]> cypherSecrets = new Dictionary<string, byte[]>();

        private readonly Aes cipher;

        private readonly IDataBaseFactory dbFactory;

        public CryptService(IDataBaseFactory dbFactory)
        {
            this.dbFactory = dbFactory;

            cipher = new AesManaged
            {
                KeySize = 256,
                BlockSize = 128,
                Padding = PaddingMode.ISO10126,
                Mode = CipherMode.CBC
            };

            Initialize();
        }

        public void AddCipherKey(string key)
        {
            cypherSecrets.Add(key, GenerateSecurityKey());
        }

        public void AddCipherKey(string key, byte[] securityKey)
        {
            cypherSecrets.Add(key, securityKey);
        }

        public void AddCipherKey(string key, string securityKey)
        {
            byte[] bytes = Encoding.UTF8.GetBytes(securityKey);

            if (bytes.Length < 32)
                throw new Exception("Cipher key have to be 32 bytes length");

            if (bytes.Length > 32)
                bytes = new Span<byte>(bytes).Slice(0, 32).ToArray();

            AddCipherKey(key, bytes);
        }

        private static string ToUrlSafeBase64(byte[] input)
        {
            return Convert.ToBase64String(input).Replace("+", "-").Replace("/", "_");
        }

        private static byte[] FromUrlSafeBase64(string input)
        {
            return Convert.FromBase64String(input.Replace("-", "+").Replace("_", "/"));
        }


        public string Crypt(string cipherName, string text)
        {
            var IV = GenerateIV();
            ICryptoTransform t = cipher.CreateEncryptor(cypherSecrets[cipherName], IV);
            byte[] textInBytes = Encoding.UTF8.GetBytes(text);
            byte[] result = t.TransformFinalBlock(textInBytes, 0, textInBytes.Length);

            byte[] resultPlusIV = new byte[result.Length + IV.Length];
            result.CopyTo(resultPlusIV, 0);
            IV.CopyTo(resultPlusIV, result.Length);

            return ToUrlSafeBase64(resultPlusIV);
        }

        public string Decrypt(string cipherName, string text)
        {
            byte[] textInBytes = FromUrlSafeBase64(text);
            byte[] IV = new byte[16];
            Array.Copy(textInBytes, textInBytes.Length - IV.Length, IV, 0, IV.Length);

            ICryptoTransform t = cipher.CreateDecryptor(cypherSecrets[cipherName], IV);


            byte[] result = t.TransformFinalBlock(textInBytes, 0, textInBytes.Length - 16);

            return Encoding.UTF8.GetString(result);
        }

        public async Task ResetSecret(string name)
        {
            using (var db = dbFactory.CreateDb())
            {
                var newSecret = GenerateSecurityKeyString();

                int updated = await db.CipherSecrets.Where(x => x.Name == name).Set(x => x.Secret, newSecret)
                    .UpdateAsync();

                if (updated != 1)
                    throw new SunEntityNotUpdatedException(nameof(CipherSecret), name, "Name");
            }
        }

        public async Task ResetAllSecrets()
        {
            using (var db = dbFactory.CreateDb())
            {
                var allSecrets = db.CipherSecrets.ToArray();
                int done = 0;

                foreach (var cipherSecret in allSecrets)
                {
                    cipherSecret.Secret = GenerateSecurityKeyString();
                    done += await db.UpdateAsync(cipherSecret);
                }

                if (done != allSecrets.Length)
                    throw new SunEntityNotUpdatedException(nameof(CipherSecret), "not all secrets updated");
            }
        }

        private static byte[] GenerateIV()
        {
            var IV = new byte[16];
            CryptoRandomizer.CryptoProvider.GetBytes(IV);
            return IV;
        }

        private static byte[] GenerateSecurityKey()
        {
            var securityKey = new byte[32];
            CryptoRandomizer.CryptoProvider.GetBytes(securityKey);
            return securityKey;
        }

        public static string GenerateSecurityKeyString()
        {
            return Encoding.Unicode.GetString(GenerateSecurityKey());
        }

        public void Initialize()
        {
            cypherSecrets.Clear();
            using (var db = dbFactory.CreateDb())
            {
                foreach (var x in db.CipherSecrets)
                    AddCipherKey(x.Name, x.Secret);
            }
        }

        public void Reset()
        {
            Initialize();
        }
    }
}
