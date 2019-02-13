using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.Extensions.DependencyInjection;

namespace SunEngine.Configuration.AddServices
{
    public static class AddIUrlHelperExtensions
    {
        public static void AddUrlHelper(this IServiceCollection services)
        {
            services
                .AddSingleton<IActionContextAccessor, ActionContextAccessor>();
                //.AddScoped(it =>
                //    it
                //        .GetRequiredService<IUrlHelperFactory>()
                //        .GetUrlHelper(it.GetRequiredService<IActionContextAccessor>().ActionContext));
        }
    }
}