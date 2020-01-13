using System;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;
using SunEngine.Core.Configuration.Options;

namespace SunEngine.Core.Cache.Services
{
	public class CaptchaCacheService
	{
		private readonly IMemoryCache memoryCache;
		private readonly TimeSpan captchaCacheTimeout;

		public CaptchaCacheService(IMemoryCache memoryCache,
			IOptions<CaptchaOptions> captchaSettings)
		{
			this.memoryCache = memoryCache;

			var settings = captchaSettings.Value;
			captchaCacheTimeout = TimeSpan.FromSeconds(settings.CaptchaTimeoutSeconds);
		}

		public void Cache(string token, string answer)
		{
			var options = new MemoryCacheEntryOptions
			{
				AbsoluteExpirationRelativeToNow = captchaCacheTimeout,
			};

			memoryCache.Set(token, answer, options);
		}

		public string GetCaptchaAnswer(string token)
		{
			return memoryCache.Get<string>(token);
		}

		public void InvalidateToken(string token)
		{
			memoryCache.Remove(token);
		}
	}
}