using System.Collections.Generic;
using Microsoft.Extensions.Configuration;
using SunEngine.DataBase;
using SunEngine.Models;
using SunEngine.Models.Materials;

namespace SunEngine.Test
{
    public class TestUtils
    {
        //TODO: connect to created dbase

        public static string DataBaseConnectionConfigFile = "DataBaseConnection.json";
        public static string Provider = "DataBaseConnectionSQLiteExample";
        
        public static DataBaseConnection GetTestDataBaseConnection()
        {
            var configuration = new ConfigurationBuilder()
                .AddJsonFile(DataBaseConnectionConfigFile, optional: false, reloadOnChange: true)
                .Build();

            var dataBaseConfiguration = configuration.GetSection(Provider);
            var providerName = dataBaseConfiguration["Provider"];
            var connectionString = dataBaseConfiguration["ConnectionString"];

            return  new DataBaseConnection(providerName,connectionString);
        }

        public static DataBaseFactory GetTestDataBaseFactory()
        {
            var configuration = new ConfigurationBuilder()
                .AddJsonFile(DataBaseConnectionConfigFile, optional: false
, reloadOnChange: true)
                .Build();

            var dataBaseConfiguration = configuration.GetSection(Provider);
            var providerName = dataBaseConfiguration["Provider"];
            var connectionString = dataBaseConfiguration["ConnectionString"];
            return new DataBaseFactory(providerName,connectionString, new DbMappingSchema());
        }

        public static Category TestableValidCategory = new Category()
        {
            Name = "TestCategory",
            AreaRoot = true,
            Description = "Testable",
            Header = "Test",
            IsDeleted = false,
            IsFolder = false,
            IsHidden = false,
            IsMaterialsContainer = false,
            Id = 28,
            ParentId = 1,
            Title = "Тест"
        };
    }
}
