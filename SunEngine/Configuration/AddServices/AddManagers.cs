using Microsoft.Extensions.DependencyInjection;
using SunEngine.Managers;

namespace SunEngine.Configuration.AddServices
{
    public static class AddManagersExtensions
    {
        public static void AddManagers(this IServiceCollection services)
        {
            services.AddScoped<IMaterialsManager, MaterialsManager>();
            services.AddScoped<ICommentsManager, CommentsManager>();
            services.AddScoped<IPersonalManager, PersonalManager>();
            services.AddScoped<IProfileManager, ProfileManager>();
            services.AddScoped<ITagsManager, TagsManager>();
            services.AddScoped<IAuthManager, AuthManager>();
            services.AddScoped<IAccountManager, AccountManager>();
        }
    }
}