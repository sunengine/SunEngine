using AspNetCore.CacheOutput;
using AspNetCore.CacheOutput.InMemory;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SunEngine.Authorization.ControllersAuthorization;
using SunEngine.EntityServices;
using SunEngine.Infrastructure;
using SunEngine.Options;

namespace SunEngine.Services
{
    internal static class ServiceCollectionExtensions
    {
        public static void AddScopedControllersAuthorizationServices(this IServiceCollection services)
        {
            services.AddScoped<CategoriesAuthorization>();
            
            services.AddScoped<MaterialsAuthorization>();

            services.AddScoped<MessageAuthorization>();
        }

        public static void AddScopedEntityServices(this IServiceCollection services)
        {
            services.AddScoped<ArticlesService>();
            services.AddScoped<BlogService>();
            services.AddScoped<ForumService>();  
            services.AddScoped<MaterialsService>();  
            services.AddScoped<MessagesService>();
            services.AddScoped<TagsService>();
            services.AddScoped<UserProfileService>();
            services.AddScoped<PersonalService>();
            services.AddScoped<AuthService>();
        }

        public static void AddMyInMemoryCacheOutput(this IServiceCollection services)
        {
            services.AddSingleton<ICacheKeyGenerator, GroupCacheKeyGenerator>();

            services.AddSingleton<IApiCacheOutput, InMemoryCacheOutputProvider>();
            
        }

        public static void AddSunEngineOptions(this IServiceCollection services, IConfiguration configuration)
        {
            //services.AddOptions();
            services.Configure<GlobalOptions>(configuration);
            services.Configure<EmailSenderOptions>(configuration.GetSection("email"));
            services.Configure<JwtOptions>(configuration.GetSection("Jwt"));
            services.Configure<BlogOptions>(configuration.GetSection("Blog"));
            services.Configure<ArticlesOptions>(configuration.GetSection("Articles"));
            services.Configure<ForumOptions>(configuration.GetSection("Forum"));
            services.Configure<MaterialOptions>(configuration.GetSection("Material"));
            services.Configure<MessagesOptions>(configuration.GetSection("Messages"));
            services.Configure<ImagesOptions>(configuration.GetSection("Images"));
            services.Configure<DataBaseConnectionOptions>(configuration.GetSection("DataBaseConnection"));

        }
    }  
}