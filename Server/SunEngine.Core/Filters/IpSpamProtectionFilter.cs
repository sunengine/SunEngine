using System;
using System.Net;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.DependencyInjection;
using SunEngine.Core.Cache.Services;
using SunEngine.Core.Controllers;
using SunEngine.Core.Errors;

namespace SunEngine.Core.Filters
{
    public class IpSpamProtectionFilter : SpamProtectionFilter
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
