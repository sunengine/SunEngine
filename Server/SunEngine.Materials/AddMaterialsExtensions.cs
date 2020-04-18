using Microsoft.Extensions.DependencyInjection;
using SunEngine.Core.Managers;
using SunEngine.Core.Presenters;
using SunEngine.Materials.Presenters;

namespace SunEngine.Materials
{
	public static class AddMaterialsExtensions
	{
		public static void AddMaterials(this IServiceCollection services)
		{
			services.AddScoped<IMaterialsManager, MaterialsManager>();
			services.AddScoped<ICommentsManager, CommentsManager>();
			services.AddScoped<IArticlesPresenter, ArticlesPresenter>();
			services.AddScoped<IBlogPresenter, BlogPresenter>();
			services.AddScoped<IForumPresenter, ForumPresenter>();
			services.AddScoped<IMaterialsPresenter, MaterialsPresenter>();
			services.AddScoped<IArticlesPresenter, ArticlesPresenter>();
			services.AddScoped<IBlogPresenter, BlogPresenter>();
			services.AddScoped<IForumPresenter, ForumPresenter>();
			services.AddScoped<IMaterialsPresenter, MaterialsPresenter>();
			services.AddScoped<IActivitiesPresenter, ActivitiesPresenter>();
			services.AddScoped<ICommentsPresenter, CommentsPresenter>();
		}
	}
}