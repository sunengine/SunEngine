using System;


namespace SunEngine.Cli
{
	public static class Program
	{
		/// <summary>
		/// Entry point of CLI
		/// </summary>
		public static void Main(string[] args)
		{
			InfoPrinter.PrintLogoAndVersion();

			StartupConfiguration config = new StartupConfiguration(args);
			
			if (config.PrintHelp || config.PrintVersion)
			{
				if (config.PrintHelp)
					InfoPrinter.PrintHelp();

				else if (config.PrintVersion)
					InfoPrinter.PrintVersion();

				return;
			}

			if (config.CheckNoArguments())
			{
				InfoPrinter.PrintNoArgumentsInfo();
				return;
			}

			config.InitConfigurationDirectory();

			InfoPrinter.PrintServerInfo(config.ConfigRootDir);

			if (config.CheckDatabaseAvailability)
			{ 
				if (!DataSeed().PrintDbConnectionAvailability())
					Environment.Exit(1); 
			}

			else if (ShouldUpdate(config))
			{
				if (config.Migrate)
					Migrations().Migrate();

				if (config.InitializeCoreData)
					DataSeed().SeedInitialize();

				if (config.SeedWithTestData)
					DataSeed().SeedAddTestData(config.CategoryTokensToSeed, config.SeedWithCategoryNames);
			}

			else if (config.StartServer)
				new ServerRun().RunServer(config);
			else
				InfoPrinter.PrintNoArgumentsInfo();


			Migrations.Main Migrations() => new Migrations.Main(config.ConfigRootDir);

			DataSeed.Main DataSeed() => new DataSeed.Main(config.ConfigRootDir);
		}

		// Detect if there is on of the update database command
		private static bool ShouldUpdate(StartupConfiguration startupConfiguration)
		{
			return startupConfiguration.Migrate ||
			       startupConfiguration.InitializeCoreData ||
			       startupConfiguration.SeedWithTestData;
		}
	}
}