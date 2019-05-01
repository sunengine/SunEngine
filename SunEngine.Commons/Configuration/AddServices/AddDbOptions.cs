using System.Linq;
using Microsoft.Extensions.DependencyInjection;
using SunEngine.Commons.Configuration.Options;
using SunEngine.Commons.DataBase;
using SunEngine.Commons.Services;

namespace SunEngine.Commons.Configuration.AddServices
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