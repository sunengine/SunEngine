using System;
using System.Linq;
using System.Threading.Tasks;
using LinqToDB;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Options;
using SunEngine.Core.Cache.Services;
using SunEngine.Core.Configuration.Options;
using SunEngine.Core.DataBase;
using SunEngine.Core.Models;
using SunEngine.Core.Services;
using SunEngine.Core.Utils.CustomExceptions;

namespace SunEngine.Admin.Managers
{
    public class CacheSettingsManager : DbService
    {
        private IServiceCollection serviceCollection;
        private IServiceProvider serviceProvider;
        private IContentCache contentCache;
        
        public CacheSettingsManager(IServiceCollection serviceCollection,
            IServiceProvider serviceProvider, IContentCache contentCache, DataBaseConnection db) : base(db)
        {
            this.serviceCollection = serviceCollection;
            this.serviceProvider = serviceProvider;
            this.contentCache = contentCache;
        }

        public async Task UpdateCachePolicy(CacheSettings cacheSettings)
        {
            if(!Enum.IsDefined(typeof(CachePolicy), cacheSettings.CachePolicy))
                throw new ArgumentOutOfRangeException(nameof(cacheSettings.CachePolicy), "Unknown cache policy type");

            if (cacheSettings.CachePolicy == CachePolicy.NeverPolicy) 
                cacheSettings.InvalidateCacheTime = null;
            
            serviceProvider.GetRequiredService<IOptions<CacheOptions>>()?.Value.UpdateOptions(cacheSettings);
            
            var currentSettings = await db.CacheSettings.OrderBy(x => x.Id).FirstOrDefaultAsync();
            if (currentSettings == null)
                throw new NotFoundDataException("Can`t find cache settings");
            
            currentSettings.CachePolicy = cacheSettings.CachePolicy;
            currentSettings.InvalidateCacheTime = cacheSettings.InvalidateCacheTime;
            await db.UpdateAsync(currentSettings);
            
            contentCache.Reset();
        }
    }
}