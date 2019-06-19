using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using SunEngine.Core.Utils;

namespace SunEngine.Core.Errors
{
    public class SunExceptionMiddleware
    {
        private readonly RequestDelegate next;
        private readonly ILogger<SunExceptionMiddleware> logger;

        public SunExceptionMiddleware(RequestDelegate next, ILoggerFactory loggerFactory)
        {
            this.next = next ?? throw new ArgumentNullException(nameof(next));
            logger = loggerFactory?.CreateLogger<SunExceptionMiddleware>() ?? throw new ArgumentNullException(nameof(loggerFactory));
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await next(context);
            }
            catch (SunViewException e)
            {
                await context.Response.WriteAsync(SunJson.Serialize(e.ErrorView));
                
                context.Response.StatusCode = 500;
                logger.LogError(e.ToString());
            }
            catch (Exception e)
            {
                ErrorView errorView = ErrorView.ServerError(e);
                await context.Response.WriteAsync(SunJson.Serialize(errorView));
                
                context.Response.StatusCode = 500;
                logger.LogError(e.ToString());
            }
        }
    }
}
