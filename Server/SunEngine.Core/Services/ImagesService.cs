using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Processing;
using SunEngine.Core.Configuration.Options;
using SunEngine.Core.Errors.Exceptions;

namespace SunEngine.Core.Services
{
	public interface IImagesService
	{
		Task<FileAndDir> SaveImageAsync(IFormFile file, ResizeOptions resizeOptions);
		FileAndDir SaveBitmapImage(Stream stream, ResizeOptions resizeOptions, string ext);
	}

	public class ImagesService : IImagesService
	{
		protected const int MaxSvgSizeBytes = 40 * 1024;

		protected static readonly object lockObject = new object();

		protected readonly IImagesNamesService imagesNamesService;
		protected readonly IOptionsMonitor<ImagesOptions> imagesOptions;
		protected readonly string UploadImagesDir;


		public ImagesService(
			IOptionsMonitor<ImagesOptions> imagesOptions,
			IPathService pathService,
			IImagesNamesService imagesNamesService)
		{
			this.imagesOptions = imagesOptions;
			this.imagesNamesService = imagesNamesService;
			UploadImagesDir = pathService.GetPath(PathNames.UploadImagesDirName);
		}

		public virtual async Task<FileAndDir> SaveImageAsync(IFormFile file, ResizeOptions resizeOptions)
		{
			var ext = GetAllowedExtension(file.FileName);
			if (ext == null)
				throw new SunException($"Not allowed extension");

			if (ext == ".svg" && file.Length >= MaxSvgSizeBytes)
				throw new SunException($"Svg max size is {MaxSvgSizeBytes / 1024} kb");
			else
			{
				await using var stream = file.OpenReadStream();
				if (!IsAllowedImageFormat(Image.DetectFormat(stream).Name))
					throw new SunException("Not allowed image format");

				var imageInfo = Image.Identify(stream);
				if (imageInfo.Width > imagesOptions.CurrentValue.MaxImageWidth ||
				    imageInfo.Height > imagesOptions.CurrentValue.MaxImageHeight)
					throw new SunException(
						$"Very big image. Max image height, width is {imagesOptions.CurrentValue.MaxImageHeight}, {imagesOptions.CurrentValue.MaxImageWidth}");
			}

			var fileAndDir = imagesNamesService.GetNewImageNameAndDir(ext);
			var dirFullPath = Path.Combine(UploadImagesDir, fileAndDir.Dir);
			var fullFileName = Path.Combine(dirFullPath, fileAndDir.File);

			lock (lockObject)
				if (!Directory.Exists(dirFullPath))
					Directory.CreateDirectory(dirFullPath);

			if (ext == ".svg")
			{
				await using var stream = new FileStream(fullFileName, FileMode.Create);
				await file.CopyToAsync(stream);
			}
			else
			{
				await using var stream = file.OpenReadStream();
				using var image = Image.Load(stream);
				var (width, height) = image.Size();
				if (width > resizeOptions.Size.Width || height > resizeOptions.Size.Height)
					image.Mutate(x => x.Resize(resizeOptions));

				image.Save(fullFileName);
			}

			return fileAndDir;
		}

		public virtual FileAndDir SaveBitmapImage(Stream stream, ResizeOptions resizeOptions, string ext)
		{
			using var image = Image.Load(stream);

			var fileAndDir = imagesNamesService.GetNewImageNameAndDir(ext);
			var dirFullPath = Path.Combine(UploadImagesDir, fileAndDir.Dir);

			lock (lockObject)
				if (!Directory.Exists(dirFullPath))
					Directory.CreateDirectory(dirFullPath);

			var fullFileName = Path.Combine(dirFullPath, fileAndDir.File);

			var (width, height) = image.Size();
			if (width > resizeOptions.Size.Width || height > resizeOptions.Size.Height)
				image.Mutate(x => x.Resize(resizeOptions));

			image.Save(fullFileName);

			return fileAndDir;
		}

		private string GetAllowedExtension(string fileName)
		{
			var ext = Path.GetExtension(fileName).ToLower();
			ext = ext == ".jpeg" ? ".jpg" : ext;

			var allowedExtensions = new List<string> {".jpg", ".png"};
			if (imagesOptions.CurrentValue.AllowGifUpload)
				allowedExtensions.Add(".gif");

			if (imagesOptions.CurrentValue.AllowSvgUpload)
				allowedExtensions.Add(".svg");

			return allowedExtensions.FirstOrDefault(x => x == ext);
		}

		private bool IsAllowedImageFormat(string imageFormat)
		{
			var allowedFormats = new List<string> {"JPEG", "PNG"};
			if (imagesOptions.CurrentValue.AllowGifUpload)
				allowedFormats.Add("GIF");

			return allowedFormats.Contains(imageFormat);
		}
	}
}