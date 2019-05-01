namespace SunEngine.Commons.Models
{
    public enum CachePolicy
    {
        AlwaysPolicy,
        NeverPolicy,
        CustomPolicy
    }
    
    public class CacheSettings
    {
        public long Id { get; set; }
        
        public CachePolicy CachePolicy { get; set; }
        
        public int? InvalidateCacheTime { get; set; }
    }
}