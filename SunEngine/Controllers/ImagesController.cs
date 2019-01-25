using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing;
using SixLabors.Primitives;
using SunEngine.Commons.Models;
using SunEngine.EntityServices;
using SunEngine.Options;
using SunEngine.Services;
using SixLabors.Fonts;
using SixLabors.ImageSharp.Formats;
using SixLabors.ImageSharp.Formats.Jpeg;
using SixLabors.Shapes;
using FontStyle = SixLabors.Fonts.FontStyle;
using Path = System.IO.Path;
using System;
using System.Numerics;
using Microsoft.Extensions.Caching.Memory;
using SixLabors.Fonts;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Processing.Processors.Drawing;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing;
using SixLabors.Primitives;
using SixLabors.Shapes;

namespace SunEngine.Controllers
{
    [Authorize]
    public class ImagesController : BaseController
    {
        private readonly ImagesOptions imagesOptions;
        private readonly PersonalService personalService;
        private readonly ImagesService imagesService;

        public ImagesController(
            IOptions<ImagesOptions> imagesOptions,
            PersonalService personalService,
            ImagesService imagesService,
            CaptchaService captchaService,
            UserManager<User> userManager) : base(userManager)
        {
            this.imagesOptions = imagesOptions.Value;
            this.personalService = personalService;
            this.imagesService = imagesService;
        }

        [HttpPost]
        [RequestSizeLimit(1024*1024*8)]
        public async Task<IActionResult> UploadImage(IFormFile file)
        {
            if (file.Length == 0)
            {
                return BadRequest();
            }

            ResizeOptions ro = new ResizeOptions
            {
                Mode = ResizeMode.Max,
                Size = new Size(imagesOptions.MaxWidthPixels, imagesOptions.MaxHeightPixels),
            };
            FileAndDir fileAndDir = await imagesService.SaveImageAsync(file, ro);
            if (fileAndDir == null)
            {
                return BadRequest();
            }

            return Ok(new {FileName = fileAndDir.Path});
        }


        [HttpPost]
        [RequestSizeLimit(1024*1024*8)]
        public async Task<IActionResult> UploadUserPhoto(IFormFile file)
        {
            if (file.Length == 0)
            {
                return BadRequest();
            }

            ResizeOptions roPhoto = new ResizeOptions
            {
                Position = AnchorPositionMode.Center,
                Mode = ResizeMode.Crop,
                Size = new Size(imagesOptions.PhotoMaxWidthPixels, imagesOptions.PhotoMaxWidthPixels),
            };
            FileAndDir fileAndDirPhoto = await imagesService.SaveImageAsync(file, roPhoto);
            if (fileAndDirPhoto == null)
            {
                return BadRequest();
            }


            ResizeOptions roAvatar = new ResizeOptions
            {
                Position = AnchorPositionMode.Center,
                Mode = ResizeMode.Crop,
                Size = new Size(imagesOptions.PhotoMaxWidthPixels, imagesOptions.PhotoMaxWidthPixels),
            };
            FileAndDir fileAndDirAvatar = await imagesService.SaveImageAsync(file, roAvatar);
            if (fileAndDirAvatar == null)
            {
                return BadRequest();
            }

            await personalService.SetPhotoAndAvatarAsync(User.UserId, fileAndDirPhoto.Path, fileAndDirAvatar.Path);

            return Ok();
        }

        
    }
}