using System;
using System.IO;
using System.Reflection;
using Microsoft.AspNetCore.Hosting;

namespace SunEngine.Core.Configuration.Options
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

        public string SiteApi { get; set; }

        public (string schema, string host) GetSchemaAndHostApi()
        {
            if (SiteApi.StartsWith("http://"))
            {
                return ("http", SiteApi.Substring(7));
            }

            if (SiteApi.StartsWith("https://"))
            {
                return ("https", SiteApi.Substring(8));
            }

            throw new Exception("SiteApi must starts with 'http://' or 'https://'");
        }

        public string SiteName { get; set; }

        public bool ShowExceptions { get; set; }

        /* public string WwwRootDir { get; set; }
        public string WwwRootDirPath => Path.Combine(System.IO.Directory.GetCurrentDirectory(), WwwRootDir);
        public string StaticsDir { get; set; }
        public string StaticsDirPath => Path.Combine(System.IO.Directory.GetCurrentDirectory(), WwwRootDir, StaticsDir);
        public string AllSkinsDir { get; set; }
        public string AllSkinsDirPath => Path.Combine(System.IO.Directory.GetCurrentDirectory(), WwwRootDir, StaticsDir, AllSkinsDir);
        public string CurrentSkinDir { get; set; }        
        public string CurrentSkinDirPath => Path.Combine(System.IO.Directory.GetCurrentDirectory(), WwwRootDir, StaticsDir, CurrentSkinDir);*/

    }
}