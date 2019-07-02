using System;

namespace SunEngine.Core.Configuration.Options
{
    public class SanitizerOptions
    {
        public string[] AllowedTags { get; set; } = Array.Empty<string>();
        
        public string[] AllowedAttributes { get; set; } = Array.Empty<string>();
        
        public string[] AllowedClasses { get; set; } = Array.Empty<string>();
        
        public string[] AllowedCssProperties { get; set; } = Array.Empty<string>();
        
        public string[] AllowedVideoDomains { get; set; } = Array.Empty<string>();
    }
}
