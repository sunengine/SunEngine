namespace SunEngine.Commons.Models
{
    public enum CachePolicy
    {
        AlwaysPolicy = 0,
        NeverPolicy,
        CustomPolicy
    }
    
    public class CacheSettings
    {
        public long Id { get; set; }
        public CachePolicy CachePolicy { get; set; }
    }
}