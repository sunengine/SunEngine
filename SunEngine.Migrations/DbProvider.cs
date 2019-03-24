using System;
using FluentMigrator.Runner;


namespace Migrations
{
    public static class DbProvider
    {
        public static string Name { get; private set; }

        public static bool IsPostgre { get; private set; }

        public static void Initialize(string name)
        {
            Name = name;

            if (name.StartsWith("Postgre"))
            {
                IsPostgre = true;
            }
        }

        public static IMigrationRunnerBuilder AddDb(this IMigrationRunnerBuilder rb)
        {
            if (string.Equals(Name, "Sqlite", StringComparison.OrdinalIgnoreCase))
                rb.AddSQLite();
            else if (Name.Equals("Postgres", StringComparison.OrdinalIgnoreCase))
                rb.AddPostgres();
            else if (Name.Equals("MySql", StringComparison.OrdinalIgnoreCase))
                rb.AddMySql5();
            else if (Name.Equals("MySql4", StringComparison.OrdinalIgnoreCase))
                rb.AddMySql4();
            else if (Name.Equals("MySql5", StringComparison.OrdinalIgnoreCase))
                rb.AddMySql5();
            else if (Name.Equals("SqlServer", StringComparison.OrdinalIgnoreCase))
                rb.AddSqlServer();
            else if (Name.Equals("SqlServer2012", StringComparison.OrdinalIgnoreCase))
                rb.AddSqlServer2012();
            else if (Name.Equals("SqlServer2014", StringComparison.OrdinalIgnoreCase))
                rb.AddSqlServer2014();
            else if (Name.Equals("SqlServer2016", StringComparison.OrdinalIgnoreCase))
                rb.AddSqlServer2016();
            else
                throw new Exception("Not supported provider name: " + Name);

            return rb;
        }
    }
}