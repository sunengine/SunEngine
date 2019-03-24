using Microsoft.Extensions.DependencyInjection;
using SunEngine.Commons.Presenters;

namespace SunEngine.Commons.Configuration.AddServices
{
    public static class AddPresentersExtensions
    {
        public static void AddPresenters(this IServiceCollection services)
        {
            services.AddScoped<IArticlesPresenter, ArticlesPresenter>();
            services.AddScoped<IBlogPresenter, BlogPresenter>();
            services.AddScoped<IForumPresenter, ForumPresenter>();
            services.AddScoped<IMaterialsPresenter, MaterialsPresenter>();
            services.AddScoped<ICommentsPresenter, CommentsPresenter>();
            services.AddScoped<IPersonalPresenter, PersonalPresenter>();
            services.AddScoped<IProfilePresenter, ProfilePresenter>();
            services.AddScoped<ICategoriesPresenter, CategoriesPresenter>();
            services.AddScoped<IActivitiesPresenter, ActivitiesPresenter>();
        }
    }
}