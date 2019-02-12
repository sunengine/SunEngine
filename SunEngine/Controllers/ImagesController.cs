using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using SixLabors.ImageSharp.Processing;
using SixLabors.Primitives;
using SunEngine.Configuration.Options;
using SunEngine.Managers;
using SunEngine.Services;
using SunEngine.Stores;

namespace SunEngine.Controllers
{
    [Authorize]
    public class ImagesController : BaseController
    {
        protected readonly ImagesService imagesService;
        protected readonly ImagesOptions imagesOptions;
        protected readonly PersonalManager personalManager;

        public ImagesController(
            ImagesService imagesService,
            IOptions<ImagesOptions> imagesOptions,
            PersonalManager personalManager,
            MyUserManager userManager,
            IUserGroupStore userGroupStore) : base(userGroupStore, userManager)
        {
            this.imagesService = imagesService;
            this.imagesOptions = imagesOptions.Value;
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
                Size = new Size(imagesOptions.PhotoMaxWidthPixels, imagesOptions.PhotoMaxWidthPixels),
            };
            FileAndDir fileAndDirPhoto = await imagesService.SaveImageAsync(file, resizeOptionsPhoto);
            if (fileAndDirPhoto == null)
            {
                return BadRequest();
            }


            ResizeOptions resizeOptionsAvatar = new ResizeOptions
            {
                Position = AnchorPositionMode.Center,
                Mode = ResizeMode.Crop,
                Size = new Size(imagesOptions.AvatarSizePixels, imagesOptions.AvatarSizePixels),
            };
            FileAndDir fileAndDirAvatar = await imagesService.SaveImageAsync(file, resizeOptionsAvatar);
            if (fileAndDirAvatar == null)
            {
                return BadRequest();
            }

            await personalManager.SetPhotoAndAvatarAsync(User.UserId, fileAndDirPhoto.Path, fileAndDirAvatar.Path);

            return Ok();
        }

        protected bool CheckAllowedMaxImageSize(long fileSize)
        {
            return fileSize <= imagesOptions.ImageRequestSizeLimitBytes;
        }

        protected IActionResult MaxImageSizeFailResult()
        {
            double sizeInMb = imagesOptions.ImageRequestSizeLimitBytes / (1024d * 1024d);
            return BadRequest($"Image size is too large. Allowed max size is: {sizeInMb:F2} MB");
        }
    }
}