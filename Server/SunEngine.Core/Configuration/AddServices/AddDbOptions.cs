using Microsoft.Extensions.DependencyInjection;
using SunEngine.Core.Configuration.Options;
using SunEngine.Core.DataBase;
using SunEngine.Core.Services;

namespace SunEngine.Core.Configuration.AddServices
{
    public static class AddDbOptionsExtensions
    {
        public static void AddDbOptions(this IServiceCollection service, IDataBaseFactory dbFactory)
        {
            var dbOptionsService = new DbOptionsService(dbFactory.CreateDb());
            service.Configure<CacheOptions>(options => { options.UpdateOptions(dbOptionsService.GetCacheSettings()); });
        }
    }
}
