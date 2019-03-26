using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing;
using SunEngine.Commons.Configuration.Options;

namespace SunEngine.Commons.Services
{
    public interface IImagesService
    {
        string GetAllowedExtension(string fileName);
        Task<FileAndDir> SaveImageAsync(IFormFile file, ResizeOptions resizeOptions);
        FileAndDir SaveBitmapImage(Stream stream, ResizeOptions ro, string ext);
    }

    public class ImagesService : IImagesService
    {
        protected static readonly int MaxSvgSizeBytes = 40 * 1024;

        protected static readonly object lockObject = new object();

        protected readonly IImagesNamesService imagesNamesService;
        protected readonly ImagesOptions imagesOptions;
        protected readonly IHostingEnvironment env;


        public ImagesService(
            IOptions<ImagesOptions> imageOptions,
            IImagesNamesService imagesNamesService,
            IHostingEnvironment env)
        {
            this.imagesOptions = imageOptions.Value;
            this.env = env;
            this.imagesNamesService = imagesNamesService;
        }

        public virtual string GetAllowedExtension(string fileName)
        {
            string ext = Path.GetExtension(fileName).ToLower();
            if (ext == ".jpeg")
                return ".jpg";
            if (ext == ".jpg" || ext == ".png" || ext == ".gif")
                return ext;
            if (imagesOptions.AllowSvgUpload && ext == ".svg")
                return ext;

            return null;
        }

        public virtual async Task<FileAndDir> SaveImageAsync(IFormFile file, ResizeOptions resizeOptions)
        {
            var ext = GetAllowedExtension(file.FileName);
            if (ext == null)
            {
                throw new Exception($"Not allowed extension");
            }

            if (ext == ".svg" && file.Length >= MaxSvgSizeBytes)
            {
                throw new Exception($"Svg max size is {MaxSvgSizeBytes / 1024} kb");
            }

            var fileAndDir = imagesNamesService.GetNewImageNameAndDir(ext);
            var dirFullPath = Path.Combine(env.WebRootPath, imagesOptions.UploadDir, fileAndDir.Dir);
            var fullFileName = Path.Combine(dirFullPath, fileAndDir.File);

            lock (lockObject)
            {
                if (!Directory.Exists(dirFullPath))
                {
                    Directory.CreateDirectory(dirFullPath);
                }
            }

            if (ext == ".svg")
            {
                using (var stream = new FileStream(fullFileName, FileMode.Create))
                {
                    await file.CopyToAsync(stream);
                }
            }
            else
            {
                using (var stream = file.OpenReadStream())
                using (Image<Rgba32> image = Image.Load(stream))
                {
                    image.Mutate(x => x.Resize(resizeOptions));
                    image.Save(fullFileName);
                }
            }

            return fileAndDir;
        }

        public virtual FileAndDir SaveBitmapImage(Stream stream, ResizeOptions ro, string ext)
        {
            using (Image<Rgba32> image = Image.Load(stream))
            {
                var fileAndDir = imagesNamesService.GetNewImageNameAndDir(ext);
                var dirFullPath = Path.Combine(env.WebRootPath, imagesOptions.UploadDir, fileAndDir.Dir);

                lock (lockObject)
                {
                    if (!Directory.Exists(dirFullPath))
                    {
                        Directory.CreateDirectory(dirFullPath);
                    }
                }

                var fullFileName = Path.Combine(dirFullPath, fileAndDir.File);

                image.Mutate(x => x
                    .Resize(ro));

                image.Save(fullFileName);

                return fileAndDir;
            }
        }
    }
}