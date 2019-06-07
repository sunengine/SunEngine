using System;
using System.Collections.Generic;
using System.Linq;
using FluentMigrator.Runner;

namespace SunEngine.Migrations
{
    public static class DbProvider
    {
        public static string Name { get; private set; }

        public static bool IsPostgre { get; private set; }

        public static void Initialize(string name)
        {
            Name = name;

            IsPostgre = name.StartsWith("Postgre", StringComparison.OrdinalIgnoreCase);
        }

        public static IMigrationRunnerBuilder AddDb(this IMigrationRunnerBuilder rb)
        {
            var addDbFunctions = new Dictionary<string, Func<IMigrationRunnerBuilder>>(StringComparer.OrdinalIgnoreCase)
            {
                ["SqLite"] = rb.AddSQLite,
                ["Postgres"] = rb.AddPostgres,
                ["MySql"] = rb.AddMySql5,
                ["MySql4"] = rb.AddMySql4,
                ["MySql5"] = rb.AddMySql5,
                ["SqlServer"] = rb.AddSqlServer,
                ["SqlServer2012"] = rb.AddSqlServer2012,
                ["SqlServer2014"] = rb.AddSqlServer2014,
                ["SqlServer2016"] = rb.AddSqlServer2016
            };

            if (addDbFunctions.TryGetValue(Name, out var addDbFunc))
                return addDbFunc();

            throw new Exception("Not supported provider name: " + Name);
        }
    }
}
