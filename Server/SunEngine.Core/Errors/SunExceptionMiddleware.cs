using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using SunEngine.Core.Errors.Exceptions;
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
			logger = loggerFactory?.CreateLogger<SunExceptionMiddleware>() ??
			         throw new ArgumentNullException(nameof(loggerFactory));
		}

		public async Task Invoke(HttpContext context)
		{
			try
			{
				await next(context);
			}
			catch (SunErrorException e)
			{
				logger.LogError(e.ToString());
				context.Response.StatusCode = 500;

				await context.Response.WriteAsync(SunJson.Serialize(e.Error));
			}
			catch (Exception e)
			{
				logger.LogError(e.ToString());
				context.Response.StatusCode = 500;

				var error = Errors.ServerError(e);
				await context.Response.WriteAsync(SunJson.Serialize(error));
			}
		}
	}
}