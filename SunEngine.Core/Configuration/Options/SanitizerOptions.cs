namespace SunEngine.Core.Configuration.Options
{
    public class SanitizerOptions
    {
        public string[] AllowedTags { get; set; }
        
        public string[] AllowedAttributes { get; set; }
        
        public string[] AllowedClasses { get; set; }
        
        public string[] AllowedCssProperties { get; set; }
        
        public string[] AllowedVideoDomains { get; set; }
    }
}
