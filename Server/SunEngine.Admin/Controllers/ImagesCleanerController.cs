using System;
using SunEngine.Admin.Services;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace SunEngine.Admin.Controllers
{
	public class ImagesCleanerController : BaseAdminController
	{
		private readonly ImageCleanerAdminService imageCleanerService;

		public ImagesCleanerController(IServiceProvider serviceProvider,
			ImageCleanerAdminService imageCleanerService) : base(serviceProvider)
		{
			this.imageCleanerService = imageCleanerService;
		}

		[HttpPost]
		public async Task<IActionResult> GetAllImages()
		{
			var imagesPath = await imageCleanerService.GetImageSourcesForCleanAsync();

			return Ok(imagesPath);
		}

		[HttpPost]
		public async Task<IActionResult> DeleteImages()
		{
			var imagesDeleted = await imageCleanerService.DeleteImagesAsync();

			return Ok(new {ImagesDeleted = imagesDeleted});
		}
	}
}