using Microsoft.Extensions.DependencyInjection;
using SunEngine.Core.Presenters;

namespace SunEngine.Core.Configuration.AddServices
{
	public static class AddPresentersExtensions
	{
		public static void AddPresenters(this IServiceCollection services)
		{
			services.AddScoped<IPersonalPresenter, PersonalPresenter>();
			services.AddScoped<IProfilePresenter, ProfilePresenter>();
			services.AddScoped<ICategoriesPresenter, CategoriesPresenter>();
			services.AddScoped<ISearchPresenter, SearchPresenter>();
    }
	}
}
