using System.Threading.Tasks;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using SunEngine.Commons.Utils;

namespace SunEngine.Commons.Errors
{
    public static class SunExceptionHandler
    {
        public static async Task Handler(HttpContext context)
        {
            context.Response.StatusCode = 500;
            context.Response.ContentType = "application/json";
                    
            var exceptionHandlerPathFeature = 
                context.Features.Get<IExceptionHandlerPathFeature>();

            if (exceptionHandlerPathFeature.Error !=null)
            {
                ErrorView errorView = ErrorView.ServerError(exceptionHandlerPathFeature.Error);
                        
                await context.Response.WriteAsync(WebJson.Serialize(errorView));
            }     
        } 
    }
}