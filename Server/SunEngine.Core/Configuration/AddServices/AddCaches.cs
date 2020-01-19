using System;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using SunEngine.Core.Cache.CachePolicy;
using SunEngine.Core.Cache.Services;
using SunEngine.Core.Configuration.Options;
using SunEngine.Core.DataBase;
using SunEngine.Core.Errors.Exceptions;

namespace SunEngine.Core.Configuration.AddServices
{
	public static class AddStoresExtensions
	{
		/// <summary>
		/// Add Singleton cache services
		/// </summary>
		public static void AddCaches(this IServiceCollection services, IDataBaseFactory dataBaseFactory)
		{
			services.AddSingleton<IRolesCache>(new RolesCache(dataBaseFactory));

			services.AddSingleton<ICategoriesCache, CategoriesCache>();

			services.AddSingleton<IMenuCache, MenuCache>();

			services.AddSingleton<IComponentsCache, ComponentsCache>();

			services.AddSingleton<IContentCache, CategoryContentCache>();

			services.AddSingleton<CacheKeyGenerator>();

			services.AddSingleton<SpamProtectionCache>();

			services.AddSingleton<IMailTemplatesCache, MailTemplatesCache>();

			services.AddSingleton<CaptchaCacheService>();
		}

		public static void AddCachePolicy(this IServiceCollection services) => services.AddScoped(GetCachePolicy);

		private static ICachePolicy GetCachePolicy(IServiceProvider provider)
		{
			var cacheOptions = provider.GetRequiredService<IOptionsMonitor<CacheOptions>>();
			if (cacheOptions == null)
				throw new NotFoundServiceException("Cache policy must be added after loading settings from database");

			switch (cacheOptions.CurrentValue.CurrentCachePolicy)
			{
				case CachePolicy.AlwaysPolicy:
					return new AlwaysCachePolicy();
				case CachePolicy.NeverPolicy:
					return new NeverCachePolicy();
				case CachePolicy.CustomPolicy:
					return new CustomCachePolicy();
				default:
					throw new InvalidOperationException("No operation is defined for this cache policy");
			}
		}
	}
}