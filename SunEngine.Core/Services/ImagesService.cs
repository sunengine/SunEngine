using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Processing;
using SunEngine.Core.Configuration.Options;

namespace SunEngine.Core.Services
{
    public interface IImagesService
    {
        string GetAllowedExtension(string fileName);
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
            UploadImagesDir = pathService.MakePath(this.imagesOptions.CurrentValue.ImagesUploadDir);
        }

        public virtual string GetAllowedExtension(string fileName)
        {
            string ext = Path.GetExtension(fileName).ToLower();
            switch (ext)
            {
                case ".jpeg":
                    return ".jpg";
                case ".jpg":
                case ".png":
                    return ext;
                case ".gif":
                    return imagesOptions.CurrentValue.AllowGifUpload ? ext : null;
                case ".svg":
                    return imagesOptions.CurrentValue.AllowSvgUpload ? ext : null;
            }

            return null;
        }

        public virtual async Task<FileAndDir> SaveImageAsync(IFormFile file, ResizeOptions resizeOptions)
        {
            var ext = GetAllowedExtension(file.FileName);
            if (ext == null)
                throw new Exception($"Not allowed extension");

            if (ext == ".svg" && file.Length >= MaxSvgSizeBytes)
                throw new Exception($"Svg max size is {MaxSvgSizeBytes / 1024} kb");

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
                if(width > resizeOptions.Size.Width || height > resizeOptions.Size.Height)
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
            if(width > resizeOptions.Size.Width || height > resizeOptions.Size.Height)
                image.Mutate(x => x.Resize(resizeOptions));

            image.Save(fullFileName);

            return fileAndDir;
        }
    }
}
