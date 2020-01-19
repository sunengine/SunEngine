using System.IO;
using Microsoft.Extensions.Configuration;
using SunEngine.Admin.Services;
using SunEngine.Core.Services;

namespace SunEngine.Cli
{
	/// <summary>
	/// Class to prepare directory and files site structure
	/// </summary>
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
			EnsureUploadImagesDirCreated();
			UpdateConfigJsOnClientDir();
		}

		public void EnsureUploadImagesDirCreated()
		{
			var uploadImagesPath = pathService.GetPath(PathNames.UploadImagesDirName);
			if (!Directory.Exists(uploadImagesPath))
				Directory.CreateDirectory(uploadImagesPath);
		}

		/// <summary>
		/// Update config.js on client '/wwwroot/statics/config.js' to actual config values
		/// </summary>
		public void UpdateConfigJsOnClientDir()
		{
			new ConfigurationAdminService(pathService, configuration).UpdateClientScripts();
		}
	}
}