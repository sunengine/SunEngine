using System;

namespace SunEngine.Core.Configuration.Options
{
	public class UrlsOptions
	{
		public string Site { get; set; }
		public string SiteApi { get; set; }
		public string UploadImages { get; set; }
		public string Skins { get; set; }
		public string PartialSkins { get; set; }
		public bool IsHttps => GetSchemaAndHostApi().schema == "https";
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
		
		public (string schema, string host) GetSchemaAndHost()
		{
			if (Site.StartsWith("http://"))
			{
				return ("http", Site.Substring(7));
			}

			if (Site.StartsWith("https://"))
			{
				return ("https", Site.Substring(8));
			}

			throw new Exception("SiteUrl must starts with 'http://' or 'https://'");
		}
	}

	public class GlobalOptions
	{
		public string SiteName { get; set; }
		public bool DisallowRegistration { get; set; }
		public bool ReadOnlyMode { get; set; }
	}
}