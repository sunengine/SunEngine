using System.Threading.Tasks;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using SunEngine.Core.Utils;

namespace SunEngine.Core.Errors
{
	public static class SunExceptionHandler
	{
		public static async Task Handler(HttpContext context)
		{
			context.Response.StatusCode = 500;
			context.Response.ContentType = "application/json";


			var exceptionHandlerPathFeature =
				context.Features.Get<IExceptionHandlerPathFeature>();

			switch (exceptionHandlerPathFeature.Error)
			{
				case null:
					return;
				default:
					var error = Errors.ServerError(exceptionHandlerPathFeature.Error);
					await context.Response.WriteAsync(SunJson.Serialize(error));
					break;
			}
		}
	}
}