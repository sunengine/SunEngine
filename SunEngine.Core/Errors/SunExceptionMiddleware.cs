using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using SunEngine.Core.Utils;

namespace SunEngine.Core.Errors
{
    public class SunExceptionMiddleware
    {
        private readonly RequestDelegate _next;
        //private readonly ILogger<SunExceptionMiddleware> _logger;

        public SunExceptionMiddleware(RequestDelegate next, ILoggerFactory loggerFactory)
        {
            _next = next ?? throw new ArgumentNullException(nameof(next));
            //_logger = loggerFactory?.CreateLogger<SunExceptionMiddleware>() ?? throw new ArgumentNullException(nameof(loggerFactory));
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (SunViewException e)
            {
                await context.Response.WriteAsync(SunJson.Serialize(e.ErrorView));
            }
            catch (Exception e)
            {
                ErrorView errorView = ErrorView.ServerError(e);
                await context.Response.WriteAsync(SunJson.Serialize(errorView));
            }
        }
    }
}
