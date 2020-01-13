using Microsoft.Extensions.Configuration;
using SunEngine.Core.DataBase;

namespace SunEngine.Core.Configuration
{
	public class ConfigDbSource : IConfigurationSource
	{
		protected readonly IDataBaseFactory dataBaseFactory;
		protected ConfigDbProvider configDbProvider;

		public ConfigDbSource(ConfigDbProvider configDbProvider, IDataBaseFactory dataBaseFactory)
		{
			this.dataBaseFactory = dataBaseFactory;
			this.configDbProvider = configDbProvider;
		}

		public IConfigurationProvider Build(IConfigurationBuilder builder)
		{
			return configDbProvider;
		}
	}
}