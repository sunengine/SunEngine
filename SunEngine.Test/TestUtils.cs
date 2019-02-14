using Microsoft.Extensions.Configuration;
using SunEngine.DataBase;

namespace SunEngine.Test
{
    public static class TestUtils
    {
        //TODO: connect to created dbase

        private static string DataBaseConnectionConfigFile = "DataBaseConnection.json";
        private static string Provider = "DataBaseConnectionSQLiteExample";
        
        public static DataBaseConnection GetTestDataBaseConnection()
        {
            var configuration = new ConfigurationBuilder()
                .AddJsonFile(DataBaseConnectionConfigFile, optional: false, reloadOnChange: true)
                .Build();

            var dataBaseConfiguration = configuration.GetSection("DataBaseConnectionSQLiteExample");
            var providerName = dataBaseConfiguration["Provider"];
            var connectionString = dataBaseConfiguration["ConnectionString"];

            return  new DataBaseConnection(providerName,connectionString);
        }
    }
}
