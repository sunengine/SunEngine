using System;

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

		public string UploadImagesUrl { get; set; }

		public string SkinsUrl { get; set; }

		public string PartialSkinsUrl { get; set; }
		public bool FileServer { get; set; }

		public bool UpdateClientFilesOnConfigChanges { get; set; }
		public bool IsHttps => GetSchemaAndHostApi().schema == "https";
	}
}