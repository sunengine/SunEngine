using Microsoft.AspNetCore.Identity;

namespace SunEngine.Utils
{
    public  class Normalizer : ILookupNormalizer
    {
        public static readonly Normalizer Singleton = new Normalizer();
        
        public string Normalize(string key)
        {
            return key.ToLower();
        }
    }
}