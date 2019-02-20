using Microsoft.AspNetCore.Identity;

namespace SunEngine.Utils
{
    public static class FieldNormalizer
    {
        public static readonly FieldNormalizerLowercase Singleton = new FieldNormalizerLowercase();

        public static string Normalize(string key)
        {
            return Singleton.Normalize(key);
        }
    }

    public class FieldNormalizerLowercase : ILookupNormalizer
    {
        public string Normalize(string key)
        {
            return key.ToLower();
        }
    }
}