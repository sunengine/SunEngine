using Microsoft.AspNetCore.Identity;

namespace SunEngine.Core.Utils
{
    public static class Normalizer
    {
        public static readonly NormalizerLowercase Singleton = new NormalizerLowercase();


        public static string Normalize(string key)
        {
            return Singleton.NormalizeName(key);
        }
    }

    public class NormalizerLowercase : ILookupNormalizer
    {
        public string NormalizeEmail(string email)
        {
            throw new System.NotImplementedException();
        }

        public string NormalizeName(string name)
        {
            return name.ToLower();
        }
    }
}
