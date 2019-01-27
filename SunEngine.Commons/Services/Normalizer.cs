using Microsoft.AspNetCore.Identity;

namespace SunEngine.Commons.Services
{
    public class Normalizer : ILookupNormalizer
    {
        public static readonly Normalizer Singleton = new Normalizer();
        
        public string Normalize(string key)
        {
            return key.ToLower();
        }
    }
}