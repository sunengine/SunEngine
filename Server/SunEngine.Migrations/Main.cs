using System;
using System.IO;
using FluentMigrator.Runner;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Npgsql;
using SunEngine.Migrations.Migrations;


namespace SunEngine.Migrations
{
	/// <summary>
	/// Class to migrate database or create initial structure with FluentMigrator Framework
	/// </summary>
	public class Main
	{
		private const string DataBaseConnectionFileName = "DataBaseConnection.json";

		private readonly string DataBaseConnectionFilePath;


		public Main(string configDirPath)
		{
			DataBaseConnectionFilePath = Path.Combine(configDirPath, DataBaseConnectionFileName);
		}


		public void Migrate()
		{
			var serviceProvider = CreateServices();

			using var scope = serviceProvider.CreateScope();

			UpdateDatabase(scope.ServiceProvider);
		}

		/// <summary>
		/// Configure the dependency injection services
		/// </summary>
		private IServiceProvider CreateServices()
		{
			var configuration = new ConfigurationBuilder()
				.AddJsonFile(DataBaseConnectionFilePath, false, false)
				.Build();

			var dataBaseConfiguration = configuration.GetSection("DataBaseConnection");
			var providerName = dataBaseConfiguration["FluentMigratorProvider"];
			DbProvider.Initialize(providerName);
			var connectionString = dataBaseConfiguration["ConnectionString"];
			CheckDbConnectionAndExitIfFailed(connectionString);

			return new ServiceCollection()
				.AddFluentMigratorCore()
				.ConfigureRunner(rb =>
				{
					rb
						.AddDb()
						.WithGlobalConnectionString(connectionString)
						.ScanIn(typeof(Initial).Assembly).For.Migrations();
				})
				.AddLogging(lb => lb.AddFluentMigratorConsole())
				.BuildServiceProvider(false);
		}

		/// <summary>
		/// Update the database
		/// </summary>
		private static void UpdateDatabase(IServiceProvider serviceProvider)
		{
			var runner = serviceProvider.GetRequiredService<IMigrationRunner>();

			if (runner.HasMigrationsToApplyUp())
				runner.MigrateUp();
			else
				Console.WriteLine("No migration needed.");
		}

		private static void CheckDbConnectionAndExitIfFailed(string connectionString)
		{
			switch (DbProvider.ProviderType)
			{
				case DbProviderType.Postgres:
					try
					{
						using var connection = new NpgsqlConnection(connectionString);
						using var cmd = new NpgsqlCommand("SELECT 1", connection);
						connection.Open();
						cmd.ExecuteScalar();
					}
					catch (Exception e)
					{
						Error(e);
					}

					break;
				/*case DbProviderType.MySql:
					try
					{
						using var connection = new MySqlConnection(connectionString);
						using var cmd = new MySqlCommand("SELECT 1", connection);
						connection.Open();
						cmd.ExecuteScalar();
					}
					catch (Exception e)
					{
						Error(e);
					}

					break;*/
			}

			void Error(Exception e)
			{
				Console.WriteLine();
				Console.WriteLine(e);
				Console.WriteLine();
				Console.ForegroundColor = ConsoleColor.Red;
				Console.WriteLine("Connection error.\nCheck if database exists and connection string correct.");
				Console.ResetColor();
				Console.WriteLine();
				Environment.Exit(1);
			}
		}
	}
}