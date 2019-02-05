using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SunEngine.Admin.Services;
using SunEngine.Authorization;
using SunEngine.EntityServices;
using SunEngine.Options;

namespace SunEngine.Services
{
    internal static class ServiceCollectionExtensions
    {
        public static void AddAuthorization(this IServiceCollection services)
        {
            services.AddScoped<CategoriesAuthorization>();
            services.AddScoped<MaterialsAuthorization>();
            services.AddScoped<MessageAuthorization>();
            services.AddScoped<AuthService>();
        }

        public static void AddManagers(this IServiceCollection services)
        {
            services.AddScoped<MaterialsManager>();  
            services.AddScoped<MessagesManager>();  
            services.AddScoped<PersonalManager>();
            services.AddScoped<ProfileManager>();
            services.AddScoped<TagsManager>();
        }
        
        public static void AddPresenters(this IServiceCollection services)
        {
            services.AddScoped<ArticlesPresenter>();
            services.AddScoped<BlogPresenter>();
            services.AddScoped<ForumPresenter>(); 
            services.AddScoped<MaterialsPresenter>();
            services.AddScoped<MessagesPresenter>();
            services.AddScoped<PersonalPresenter>(); 
            services.AddScoped<ProfilePresenter>();
        }


        public static void AddMyOptions(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddOptions();
            services.Configure<GlobalOptions>(configuration.GetSection("Global"));
            services.Configure<EmailSenderOptions>(configuration.GetSection("Email"));
            services.Configure<JwtOptions>(configuration.GetSection("Jwt"));
            services.Configure<BlogOptions>(configuration.GetSection("Blog"));
            services.Configure<ArticlesOptions>(configuration.GetSection("Articles"));
            services.Configure<ForumOptions>(configuration.GetSection("Forum"));
            services.Configure<MaterialOptions>(configuration.GetSection("Material"));
            services.Configure<MessagesOptions>(configuration.GetSection("Messages"));
            services.Configure<ImagesOptions>(configuration.GetSection("Images"));
            services.Configure<DataBaseConnectionOptions>(configuration.GetSection("DataBaseConnection"));
            services.Configure<CaptchaOptions>(configuration.GetSection("Captcha"));
        }

        public static void AddImagesServices(this IServiceCollection services)
        {
            services.AddSingleton<IImagesNamesService, ImagesNamesService>();
            services.AddSingleton<ImagesService>();
        }
        
        public static void AddAdminServices(this IServiceCollection services)
        {
            services.AddScoped<CategoriesAdminService>();
            services.AddScoped<GroupsAdminService>();
        }
        
        public static void AddCryptServices(this IServiceCollection services)
        {
            CryptService cryptService = new CryptService();
            cryptService.AddCryptorKey(CaptchaService.CryptserviceName);
            services.AddSingleton(cryptService);
        }
    }  
}