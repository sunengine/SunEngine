using System;
using System.Collections.Generic;
using FluentMigrator.Runner;

namespace SunEngine.Migrations
{
	public enum DbProviderType
	{
		Undefined,
		Postgres,
		MySql,
		MsSql
	}

	public static class DbProvider
	{
		public static string Name { get; private set; }
		public static DbProviderType ProviderType { get; private set; }
		public static bool IsPostgres => ProviderType == DbProviderType.Postgres;
		public static bool IsMySql => ProviderType == DbProviderType.MySql;
		public static bool IsMsSql => ProviderType == DbProviderType.MsSql;

		public static void Initialize(string name)
		{
			Name = name;

			var nameLower = Name.ToLower();

			if (nameLower.StartsWith("postgres"))
				ProviderType = DbProviderType.Postgres;

			else if (nameLower.StartsWith("mysql"))
				ProviderType = DbProviderType.MySql;

			else if (nameLower.StartsWith("sqlserver"))
				ProviderType = DbProviderType.MsSql;
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