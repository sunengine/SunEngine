using System.IO;
using System.Linq;
using Microsoft.Extensions.Configuration;
using SunEngine.Admin.Services;
using SunEngine.Core.Services;
using SunEngine.Core.Utils;

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

        public void DoAll()
        {
            CheckOrCreateUploadImages();
            UpdateClientConfigJs();
        }

        public void CheckOrCreateUploadImages()
        {
            var uploadImagesPath = pathService.GetPath(PathNames.UploadImagesDirName);
            if (!Directory.Exists(uploadImagesPath))
                Directory.CreateDirectory(uploadImagesPath);
        }

        public void UpdateClientConfigJs()
        {
            new ConfigurationAdminService(pathService, configuration).UpdateClientScripts();
        }
    }
}
