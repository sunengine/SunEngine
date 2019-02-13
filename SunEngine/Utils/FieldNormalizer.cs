using Microsoft.AspNetCore.Identity;

namespace SunEngine.Utils
{
    public  class FieldNormalizer : ILookupNormalizer
    {
        public static readonly FieldNormalizer Singleton = new FieldNormalizer();
        
        public string Normalize(string key)
        {
            return key.ToLower();
        }
    }
}