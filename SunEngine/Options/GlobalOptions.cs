using System;

namespace SunEngine.Options
{
    public class GlobalOptions
    {
        public string SiteUrl { get; set; }

        public (string schema, string host) GetSchemaAndHost()
        {
            if (SiteUrl.StartsWith("http://"))
            {
                return ("http", SiteUrl.Substring(7));
            }
            if (SiteUrl.StartsWith("https://"))
            {
                return ("https", SiteUrl.Substring(8));
            }
            throw new Exception("SiteUrl must starts with 'http://' or 'https://'");
        }
        
    }
}