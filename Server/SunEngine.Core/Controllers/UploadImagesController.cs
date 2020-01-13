using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using SixLabors.ImageSharp.Processing;
using SixLabors.Primitives;
using SunEngine.Core.Configuration.Options;
using SunEngine.Core.Managers;
using SunEngine.Core.Services;

namespace SunEngine.Core.Controllers
{
	/// <summary>
	/// Upload images controller
	/// </summary>
	[Authorize]
	public class UploadImagesController : BaseController
	{
		protected readonly IImagesService imagesService;
		protected readonly IOptionsMonitor<ImagesOptions> imagesOptions;
		protected readonly IPersonalManager personalManager;

		public UploadImagesController(
			IImagesService imagesService,
			IOptionsMonitor<ImagesOptions> imagesOptions,
			IPersonalManager personalManager,
			IServiceProvider serviceProvider) : base(serviceProvider)
		{
			this.imagesService = imagesService;
			this.imagesOptions = imagesOptions;
			this.personalManager = personalManager;
		}


		[HttpPost]
		public virtual async Task<IActionResult> UploadImage(IFormFile file)
		{
			if (file.Length == 0)
				return BadRequest();

			if (!CheckAllowedMaxImageSize(file.Length))
				return MaxImageSizeFailResult();


			ResizeOptions ro = new ResizeOptions
			{
				Mode = ResizeMode.Max,
				Size = new Size(imagesOptions.CurrentValue.ResizeMaxWidthPixels,
					imagesOptions.CurrentValue.ResizeMaxHeightPixels)
			};

			FileAndDir fileAndDir = await imagesService.SaveImageAsync(file, ro);

			if (fileAndDir == null)
				return BadRequest();

			return Ok(new {FileName = fileAndDir.Path});
		}


		[HttpPost]
		public virtual async Task<IActionResult> UploadUserPhoto(IFormFile file)
		{
			if (file.Length == 0)
				return BadRequest();

			if (!CheckAllowedMaxImageSize(file.Length))
				return MaxImageSizeFailResult();

			ResizeOptions resizeOptionsPhoto = new ResizeOptions
			{
				Position = AnchorPositionMode.Center,
				Mode = ResizeMode.Crop,
				Size = new Size(imagesOptions.CurrentValue.PhotoMaxWidthPixels,
					imagesOptions.CurrentValue.PhotoMaxWidthPixels)
			};

			FileAndDir fileAndDirPhoto = await imagesService.SaveImageAsync(file, resizeOptionsPhoto);

			if (fileAndDirPhoto == null)
				return BadRequest();


			ResizeOptions resizeOptionsAvatar = new ResizeOptions
			{
				Position = AnchorPositionMode.Center,
				Mode = ResizeMode.Crop,
				Size = new Size(imagesOptions.CurrentValue.AvatarSizePixels,
					imagesOptions.CurrentValue.AvatarSizePixels)
			};
			FileAndDir fileAndDirAvatar = await imagesService.SaveImageAsync(file, resizeOptionsAvatar);
			if (fileAndDirAvatar == null)
				return BadRequest();

			await personalManager.SetPhotoAndAvatarAsync(User.UserId, fileAndDirPhoto.Path, fileAndDirAvatar.Path);

			return Ok();
		}

		protected bool CheckAllowedMaxImageSize(long fileSize)
		{
			return fileSize <= imagesOptions.CurrentValue.ImageRequestSizeLimitBytes;
		}

		protected IActionResult MaxImageSizeFailResult()
		{
			double sizeInMb = imagesOptions.CurrentValue.ImageRequestSizeLimitBytes / (1024d * 1024d);
			return BadRequest($"Image size is too large. Allowed max size is: {sizeInMb:F2} MB");
		}
	}
}