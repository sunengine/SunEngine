using System;
using System.Security.Cryptography;
using System.Text;

namespace SunEngine.Utils
{
    public static class CryptoRandomizer
    {
        public static readonly RNGCryptoServiceProvider CryptoProvider = new RNGCryptoServiceProvider();

        const string valid = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890";

        public static string GetRandomString(int length)
        {
            StringBuilder res = new StringBuilder();
            byte[] uintBuffer = new byte[sizeof(uint)];

            while (length-- > 0)
            {
                CryptoProvider.GetBytes(uintBuffer);
                uint num = BitConverter.ToUInt32(uintBuffer, 0);
                res.Append(valid[(int) (num % (uint) valid.Length)]);
            }

            return res.ToString();
        }
    }
}