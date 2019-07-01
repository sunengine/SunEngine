using SunEngine.Core.DataBase;

namespace SunEngine.Tests
{
    internal static class DefaultInit
    {
        private static string connectionString = "Host=localhost;Database=SunEngine;Username=postgres;Password=as3uD4531ff";
        private static string provider = "PostgreSQL.9.5";

        public static DataBaseConnection GetTestDataBaseConnection()
        {
            LinqToDB.Common.Configuration.Linq.AllowMultipleQuery = true;
            return new DataBaseConnection(provider, connectionString);
        }

        public static DataBaseFactory GetTestDataBaseFactory()
        {
            LinqToDB.Common.Configuration.Linq.AllowMultipleQuery = true;
            return new DataBaseFactory(provider, connectionString, new DbMappingSchema());
        }
    }
}
