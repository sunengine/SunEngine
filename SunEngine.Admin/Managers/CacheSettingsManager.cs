using System;
using System.Linq;
using System.Threading.Tasks;
using LinqToDB;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Options;
using SunEngine.Commons.Cache.CachePolicy;
using SunEngine.Commons.Configuration.Options;
using SunEngine.Commons.DataBase;
using SunEngine.Commons.Models;
using SunEngine.Commons.Services;
using SunEngine.Commons.Utils;
using SunEngine.Commons.Utils.CustomExceptions;

namespace SunEngine.Admin.Managers
{
    public class CacheSettingsManager : DbService
    {
        private IServiceCollection serviceCollection;
        private IServiceProvider serviceProvider;
        
        public CacheSettingsManager(IServiceCollection serviceCollection, IServiceProvider serviceProvider, DataBaseConnection db) : base(db)
        {
            this.serviceCollection = serviceCollection;
            this.serviceProvider = serviceProvider;
        }

        public async Task UpdateCachePolicy(CacheSettings cacheSettings)
        {
            if(!Enum.IsDefined(typeof(CachePolicy), cacheSettings.CachePolicy))
                throw new ArgumentOutOfRangeException();
            
            serviceProvider.GetRequiredService<IOptions<CacheOptions>>()?.Value.UpdateOptions(cacheSettings);
            
            var currentSettings = await db.CacheSettings.OrderBy(x => x.Id).FirstOrDefaultAsync();
            if (currentSettings == null)
                throw new NotFoundDataException("Can`t find cache settings");
            
            currentSettings.CachePolicy = cacheSettings.CachePolicy;
            currentSettings.InvalidateCacheTime = cacheSettings.InvalidateCacheTime;
            await db.UpdateAsync(currentSettings);
        }
    }
}