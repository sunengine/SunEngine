using Microsoft.AspNetCore.Identity;

namespace SunEngine.Core.Utils
{
    public static class Normalizer
    {
        public static readonly NormalizerLowercase Singleton = new NormalizerLowercase();


        public static string Normalize(string key)
        {
            return Singleton.Normalize(key);
        }
    }

    public class NormalizerLowercase : ILookupNormalizer
    {
        public string Normalize(string key)
        {
            return key.ToLower();
        }
    }
}
