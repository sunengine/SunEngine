using System;
using SunEngine.Core.Controllers;

namespace SunEngine.Core.Filters
{
	public class IpSpamProtectionFilter : SpamProtectionFilterBase
	{
		protected override string CacheKey => "RFIP";

		protected override string GetClientId(BaseController controller)
		{
			return controller.Request.HttpContext.Connection.RemoteIpAddress.ToString();
		}

		protected override TimeSpan CalcTimeout(BaseController controller)
		{
			return blockTimeout;
		}
	}
}