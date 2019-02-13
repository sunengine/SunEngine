using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SunEngine.Configuration.Options;

namespace SunEngine.Configuration.AddServices
{
    public static class AddOptionsExtensions
    {
        public static void AddOptions(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddOptions();
            services.Configure<GlobalOptions>(configuration.GetSection("Global"));
            services.Configure<EmailSenderOptions>(configuration.GetSection("Email"));
            services.Configure<JwtOptions>(configuration.GetSection("Jwt"));
            services.Configure<BlogOptions>(configuration.GetSection("Blog"));
            services.Configure<ArticlesOptions>(configuration.GetSection("Articles"));
            services.Configure<ForumOptions>(configuration.GetSection("Forum"));
            services.Configure<MaterialOptions>(configuration.GetSection("Materials"));
            services.Configure<MessagesOptions>(configuration.GetSection("Messages"));
            services.Configure<ImagesOptions>(configuration.GetSection("Images"));
            services.Configure<DataBaseConnectionOptions>(configuration.GetSection("DataBaseConnection"));
            services.Configure<CaptchaOptions>(configuration.GetSection("Captcha"));
        }
    }
}