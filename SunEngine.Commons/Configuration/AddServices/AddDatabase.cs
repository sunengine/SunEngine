using LinqToDB.Data;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SunEngine.Commons.DataBase;

namespace SunEngine.Commons.Configuration.AddServices
{
    public static class AddDatabaseExtensions
    {
        public static DataBaseFactory AddDatabase(this IServiceCollection services, IConfiguration configuration)
        {
            LinqToDB.Common.Configuration.Linq.AllowMultipleQuery = true;

            var dataBaseConfiguration = configuration.GetSection("DataBaseConnection");
            var providerName = dataBaseConfiguration["Provider"];
            var connectionString = dataBaseConfiguration["ConnectionString"];
            var dataProvider = DataConnection.GetDataProvider(providerName, connectionString);
            DbMappingSchema mappingSchema = new DbMappingSchema();

            services.AddScoped(x => new DataBaseConnection(dataProvider, connectionString, mappingSchema));

            var dataBaseFactory = new DataBaseFactory(dataProvider, connectionString, mappingSchema);
            
            services.AddSingleton<IDataBaseFactory>(dataBaseFactory);

            return dataBaseFactory;
        }
    }
}