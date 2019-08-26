using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SunEngine.Core.Configuration.Options;

namespace SunEngine.Core.Configuration.AddServices
{
    public static class AddOptionsExtensions
    {
        public static void AddOptions(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddOptions();
            services.Configure<GlobalOptions>(configuration.GetSection("Global"));
            services.Configure<EmailSenderOptions>(configuration.GetSection("Email"));
            services.Configure<JweOptions>(configuration.GetSection("Jwt"));
            services.Configure<BlogOptions>(configuration.GetSection("Blog"));
            services.Configure<ArticlesOptions>(configuration.GetSection("Articles"));
            services.Configure<ForumOptions>(configuration.GetSection("Forum"));
            services.Configure<MaterialsOptions>(configuration.GetSection("Materials"));
            services.Configure<CommentsOptions>(configuration.GetSection("Comments"));
            services.Configure<ImagesOptions>(configuration.GetSection("Images"));
            services.Configure<DataBaseConnectionOptions>(configuration.GetSection("DataBaseConnection"));
            services.Configure<CaptchaOptions>(configuration.GetSection("Captcha"));
            services.Configure<SchedulerOptions>(configuration.GetSection("Scheduler"));
        }
    }
}