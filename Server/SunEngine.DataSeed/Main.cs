using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using Microsoft.Extensions.Configuration;
using SunEngine.Core.DataBase;
using SunEngine.Core.Models;
using SunEngine.Core.Services;

namespace SunEngine.DataSeed
{
	/// <summary>
	/// Class to seed database with initial data with 2 modes
	/// "init" (SeedInitialize) - seed roles, users, categories
	/// "seed" (SeedAddTestData) - seed test materials and comments
	/// </summary>
	public class Main
	{
		private const string SeedCommand = "seed";

		private readonly string initDirPath;
		private readonly string providerName;
		private readonly string connectionString;
		private readonly string configDirectoryPath;

		private readonly IConfiguration configuration;

		public Main(string configDirectoryPath)
		{
			this.configDirectoryPath = configDirectoryPath;
			initDirPath = Path.Combine(configDirectoryPath, SeederPathsNames.InitDir);
			string dbSettingsFile = Path.Combine(this.configDirectoryPath, SeederPathsNames.DataBaseConnectionFile);
			string sunEngineJsonFile = Path.Combine(this.configDirectoryPath, SeederPathsNames.SunEngineJsonFile);

			configuration = new ConfigurationBuilder()
				.AddJsonFile(dbSettingsFile, false, true)
				.AddJsonFile(sunEngineJsonFile, false, true)
				.AddInMemoryCollection(new List<KeyValuePair<string, string>>()
				{
					new KeyValuePair<string, string>("Dirs:Config", configDirectoryPath)
				})
				.Build();

			var dataBaseConfiguration = configuration.GetSection("DataBaseConnection");
			providerName = dataBaseConfiguration["Linq2dbProvider"];
			connectionString = dataBaseConfiguration["ConnectionString"];
		}


		/// <summary>
		/// Initialize database with roles, users, categories from config directory
		/// </summary>
		public void SeedInitialize()
		{
			CheckDbConnectionAndExitIfFailed();

			using var db = new DataBaseConnection(providerName, connectionString);

			var dataContainer = new InitialSeeder(new PathService(configuration)).Seed();
			var databaseSeeder = new DataBaseSeeder(db, dataContainer);

			databaseSeeder.SeedInitial();
		}

		/// <summary>
		/// Seed database with materials and comment for testing purposes
		/// </summary>
		public void SeedAddTestData(IList<string> catTokens, bool titleAppendCategoryName = false)
		{
			CheckDbConnectionAndExitIfFailed();

			const string seedCommandDots = SeedCommand + ":";
			if (catTokens.Contains(SeedCommand))
				catTokens[catTokens.IndexOf(SeedCommand)] = seedCommandDots + Category.RootCategoryName;
			catTokens = catTokens.Select(x => x.Substring(seedCommandDots.Length)).ToList();

			SeedTestData();


			void SeedTestData()
			{
				using var db = new DataBaseConnection(providerName, connectionString);

				var dataContainer = new DataContainer
				{
					Categories = db.Categories.ToList(),
					Users = db.Users.ToList(),
					currentMaterialId = db.Materials.Any() ? db.Materials.Max(x => x.Id) + 1 : 1,
					currentCommentId = db.Comments.Any() ? db.Comments.Max(x => x.Id) + 1 : 1
				};

				var titlesPath = Path.Combine(this.configDirectoryPath, SeederPathsNames.InitDir, SeederPathsNames.SeedTemplatesDir, SeederPathsNames.FishTitlesFile);
				var paragraphsPath = Path.Combine(this.configDirectoryPath, SeederPathsNames.InitDir, SeederPathsNames.SeedTemplatesDir,
					SeederPathsNames.FishParagraphsFile);
				var titles = Regex.Matches(File.ReadAllText(titlesPath), "<h1>(.*?)</h1>", RegexOptions.Singleline)
					.Select(x => x.Groups[1].Value).ToList();
				var paragraphs = Regex
					.Matches(File.ReadAllText(paragraphsPath), "<p>(.*?)</p>", RegexOptions.Singleline)
					.Select(x => x.Groups[1].Value).ToList();


				var materialsSeeder = new MaterialsSeeder(dataContainer, titles, paragraphs);

				foreach (var catToken in catTokens)
				{
					var parts = catToken.Split(":");
					var categoryName = parts[0];
					int? materialsCount = null;
					if (parts.Length > 1)
						materialsCount = int.Parse(parts[1]);
					int? commentsCount = null;
					if (parts.Length > 2)
						commentsCount = int.Parse(parts[2]);

					if (materialsCount.HasValue)
						materialsSeeder.MinMaterialCount = materialsSeeder.MaxMaterialCount = materialsCount.Value;

					if (commentsCount.HasValue)
						materialsSeeder.CommentsCount = commentsCount.Value;

					materialsSeeder.TitleAppendCategoryName = titleAppendCategoryName;

					materialsSeeder.SeedCategoryAndSub(categoryName);
				}

				new DataBaseSeeder(db, dataContainer).SeedMaterials().PostSeedMaterials();
			}
		}

		protected void CheckDbConnectionAndExitIfFailed()
		{
			if (CheckDataBaseConnection(out Exception e))
				return;

			Console.WriteLine();
			Console.WriteLine(e);
			Console.WriteLine();
			Console.ForegroundColor = ConsoleColor.Red;
			Console.WriteLine("Connection error.\nCheck if database exists and connection string correct.");
			Console.ResetColor();
			Console.WriteLine();
			Environment.Exit(1);
		}

		public bool PrintDbConnectionAvailability()
		{
			if (CheckDataBaseConnection(out Exception exception))
			{
				Console.WriteLine();
				Console.ForegroundColor = ConsoleColor.DarkGreen;
				Console.WriteLine("Database is available.");
				Console.ResetColor();
				Console.WriteLine();
				return true;
			}
			else
			{
				Console.WriteLine();
				Console.WriteLine(exception);
				Console.WriteLine();
				Console.WriteLine();
				Console.ForegroundColor = ConsoleColor.Red;
				Console.WriteLine("Database is unavailable.");
				Console.ResetColor();
				Console.WriteLine();
				return false;
			}
		}


		protected bool CheckDataBaseConnection(out Exception exception)
		{
			exception = null;
			try
			{
				using var db = new DataBaseConnection(providerName, connectionString);
				using var cmd = db.CreateCommand();
				cmd.CommandText = "SELECT 100";
				cmd.ExecuteScalar();

				return true;
			}
			catch (Exception e)
			{
				exception = e;
				return false;
			}
		}
	}
}