namespace SunEngine.Core.Configuration.Options
{
    public class SanitizerOptions
    {
        public string[] AllowedTags { get; }
        
        public string[] AllowedAttributes { get; }
        
        public string[] AllowedClasses { get; }
        
        public string[] AllowedCssProperties { get; }
        
        public string[] AllowedVideoDomains { get; }
    }
}
