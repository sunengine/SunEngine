using Microsoft.Extensions.Configuration;
using SunEngine.Admin.Services;
using SunEngine.Core.Services;

namespace SunEngine.Cli
{
    public class InfrastructurePreparer
    {
        protected readonly IPathService pathService;
        protected readonly IConfigurationRoot configuration;

        public InfrastructurePreparer(
            IConfigurationRoot configuration)
        {
            pathService = new PathService(configuration);
            this.configuration = configuration;
        }

        public void CheckOrCreateCurrentSkin()
        {
        }

        public void UpdateClientConfigJs()
        {
            new ConfigurationAdminService(pathService, configuration).UpdateClientScripts();
        }
    }
}