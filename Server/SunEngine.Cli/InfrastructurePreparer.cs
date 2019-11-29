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
            CheckOrCreateCurrentSkin();
            UpdateClientConfigJs();
        }

        public void CheckOrCreateCurrentSkin()
        {
            var currentSkinPath = pathService.GetPath(PathNames.CurrentSkinDirName);
            if (Directory.Exists(currentSkinPath))
            {
                if (Directory.GetFiles(currentSkinPath).Length >= 4)
                    return;
            }
            else
                Directory.CreateDirectory(currentSkinPath);
            
            

            var allSkinsPath = pathService.GetPath(PathNames.AllSkinsDirName);

            if (!Directory.Exists(allSkinsPath))
                return;

            var allDirs = Directory.GetDirectories(allSkinsPath);
            if (allDirs.Length == 0)
                return;

            string skinDir;
            if (allDirs.Contains("Default"))
                skinDir = "Default";
            else
                skinDir = allDirs[0];

            string skinPath = Path.Combine(allSkinsPath, skinDir);

            CopyDir.Copy(skinPath, currentSkinPath);
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