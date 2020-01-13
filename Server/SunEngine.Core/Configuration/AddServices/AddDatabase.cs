using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SunEngine.Core.DataBase;

namespace SunEngine.Core.Configuration.AddServices
{
	public static class AddDatabaseExtensions
	{
		public static IServiceCollection AddDatabase(this IServiceCollection services, IConfiguration configuration,
			out IDataBaseFactory dataBaseFactory)
		{
			LinqToDB.Common.Configuration.Linq.AllowMultipleQuery = true;

			var dbFactory = DataBaseFactory.DefaultDataBaseFactory;

			services.AddSingleton<IDataBaseFactory>(dbFactory);
			services.AddScoped(x => dbFactory.CreateDb());

			dataBaseFactory = dbFactory;

			return services;
		}
	}
}