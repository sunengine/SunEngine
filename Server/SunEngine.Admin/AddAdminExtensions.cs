using Microsoft.Extensions.DependencyInjection;
using SunEngine.Admin.Managers;
using SunEngine.Admin.Presenters;
using SunEngine.Admin.Services;

namespace SunEngine.Admin
{
	public static class AddAdminExtensions
	{
		public static void AddAdmin(this IServiceCollection services)
		{
			services.AddScoped<CategoriesAdminManager>();
			services.AddScoped<IConfigurationManager, ConfigurationManager>();
			services.AddScoped<IMenuAdminManager, MenuAdminManager>();
			services.AddScoped<IComponentsAdminManager, ComponentsAdminManager>();

			services.AddScoped<IUserRolesAdminPresenter, UserRolesAdminPresenter>();
			services.AddScoped<ICategoriesAdminPresenter, CategoriesAdminPresenter>();
			services.AddScoped<IUserRolesAdminPresenter, UserRolesAdminPresenter>();
			services.AddScoped<IMenuAdminPresenter, MenuAdminPresenter>();
			services.AddScoped<IComponentsAdminPresenter, ComponentsAdminPresenter>();
			services.AddScoped<IConfigurationPresenter, ConfigurationPresenter>();

			services.AddScoped<RolesPermissionsAdminService>();
			services.AddScoped<ImageCleanerAdminService>();
			services.AddScoped<SkinsAdminService>();
			services.AddScoped<ConfigurationAdminService>();
			services.AddScoped<ICleanerManager, CleanerManager>();
		}
	}
}