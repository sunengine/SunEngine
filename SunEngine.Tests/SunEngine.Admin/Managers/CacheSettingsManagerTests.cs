using System;
using System.Linq;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using SunEngine.Admin.Managers;
using SunEngine.Core.Cache.Services;
using SunEngine.Core.Configuration.Options;
using SunEngine.Core.DataBase;
using SunEngine.Core.Models;
using Xunit;

namespace SunEngine.Tests.SunEngine.Admin.Managers
{
    public class CacheSettingsManagerTests
    {
        private CacheSettingsManager cacheSettingsManager { get; set; }
        private DataBaseConnection dbConnection { get; set; }

        public CacheSettingsManagerTests()
        {
            dbConnection = DefaultInit.GetTestDataBaseConnection();
            cacheSettingsManager = InitializeDefaultSettingsManager();
        }

        private CacheSettingsManager InitializeDefaultSettingsManager()
        {
            var serviceCollection = new ServiceCollection();
            serviceCollection.AddOptions<IOptions<CacheOptions>>();
            serviceCollection.AddMemoryCache();
            var serviceProvider = serviceCollection.BuildServiceProvider();

            var categoriesCache = new CategoriesCache(DefaultInit.GetTestDataBaseFactory());
            var cacheOptions = serviceProvider.GetRequiredService<IOptions<CacheOptions>>();
            var contentCache = new CategoryContentCache(categoriesCache, serviceProvider.GetRequiredService<IMemoryCache>(), serviceProvider.GetRequiredService<IOptions<CacheOptions>>());
            return new CacheSettingsManager(cacheOptions, contentCache, dbConnection);
        }

        [Fact]
        public async void ShouldUpdateCachePolicy()
        {
            using (var transaction = dbConnection.BeginTransaction())
            {
                var cacheSettings = dbConnection.CacheSettings.First();
                var before = cacheSettings.InvalidateCacheTime;

                cacheSettings.InvalidateCacheTime = 0;
                await cacheSettingsManager.UpdateCachePolicy(cacheSettings);

                var after = dbConnection.CacheSettings.First().InvalidateCacheTime;
                transaction.Rollback();

                Assert.NotEqual(after, before);
            }
        }

        [Fact]
        public async void ShouldThrowExceptionWhenCachePolicyIsNotDefined()
        {
            var cacheSettings = new CacheSettings{ CachePolicy = (CachePolicy)4, Id = 1, InvalidateCacheTime = 10};

            await Assert.ThrowsAsync<ArgumentOutOfRangeException>(async () =>
                await cacheSettingsManager.UpdateCachePolicy(cacheSettings));
        }
    }
}
