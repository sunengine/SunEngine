using System;
using Microsoft.AspNetCore.Http;

namespace SunEngine.Core.Configuration.Options
{
	public class SecurityOptions
	{
		public string JweIssuer { get; set; }
		public int JweLongTokenLiveTimeDays { get; set; }
		public int JweShortTokenLiveTimeMinutes { get; set; }
		public string CookieLongToken2SameSiteMode { get; set; }

		protected SameSiteMode? _cookieLongToken2SameSiteModeValue;

		public SameSiteMode CookieLongToken2SameSiteModeValue
		{
			get
			{
				if (_cookieLongToken2SameSiteModeValue == null)
				{
					if (Enum.TryParse<SameSiteMode>(CookieLongToken2SameSiteMode, out var mode))
						_cookieLongToken2SameSiteModeValue = mode;
					else
						_cookieLongToken2SameSiteModeValue = SameSiteMode.Strict;
				}

				return _cookieLongToken2SameSiteModeValue.Value;
			}
		}
	}
}