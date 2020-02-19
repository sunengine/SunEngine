using System;

namespace SunEngine.Core.Configuration.Options
{
	public class UrlPathsOptions
	{
		public string Site { get; set; }
		public string Api { get; set; }
		public string UploadImages { get; set; }
		public string Skins { get; set; }
		public string PartialSkins { get; set; }
		public bool IsHttps => GetSchemaAndHostApi().schema == "https";
		public (string schema, string host) GetSchemaAndHostApi()
		{
			if (Api.StartsWith("http://"))
			{
				return ("http", Api.Substring(7));
			}

			if (Api.StartsWith("https://"))
			{
				return ("https", Api.Substring(8));
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
}