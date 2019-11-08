using System;
using Microsoft.Extensions.Configuration;
using SunEngine.Core.DataBase;

namespace SunEngine.Core.Configuration
{
    public class ConfigDbSource : IConfigurationSource
    {
        protected readonly IDataBaseFactory dataBaseFactory;

        public ConfigDbSource(IDataBaseFactory dataBaseFactory)
        {
            this.dataBaseFactory = dataBaseFactory;
        }

        public IConfigurationProvider Build(IConfigurationBuilder builder)
        {
            return new ConfigDbProvider(dataBaseFactory);
        }
    }
}