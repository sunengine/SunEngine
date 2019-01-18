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
        public readonly ImagesService imagesService;


        public ImagesController(
            IOptions<ImagesOptions> imagesOptions,
            PersonalService personalService,
            ImagesService imagesService,
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

        /*[AllowAnonymous]
        [Produces("image/jpeg")]
        public FileStreamResult CreateCaptchaImage(string guid)
        {
            MemoryStream ms;
            using (Image<Rgba32> img = new Image<Rgba32>(400, 100))
            {
                PathBuilder pathBuilder = new PathBuilder();
                pathBuilder.SetOrigin(new PointF(500, 0));
                pathBuilder.AddBezier(new PointF(50, 450), new PointF(200, 50), new PointF(300, 50), new PointF(450, 450));
                // add more complex paths and shapes here.

                IPath path = pathBuilder.Build();

                // For production application we would recomend you create a FontCollection
                // singleton and manually install the ttf fonts yourself as using SystemFonts
                // can be expensive and you risk font existing or not existing on a deployment
                // by deployment basis.

                var font = SystemFonts.Families.First().CreateFont(39); //.CreateFont("Arial", 39, FontStyle.Regular);

                //var font = SystemFonts.CreateFont("Arial", 39, FontStyle.Regular);

                Random ran = new Random();
                string text = ran.Next(999999).ToString();
                var textGraphicsOptions =
                    new TextGraphicsOptions(true) // draw the text along the path wrapping at the end of the line
                    {
                         WrapTextWidth = path.Length
                    };
                img.Mutate(ctx => ctx
                    .Fill(Rgba32.White) // white background image
                    .Draw(Rgba32.Gray, 3, path) // draw the path so we can see what the text is supposed to be following
                    .DrawText(textGraphicsOptions, text, font, Rgba32.Black, new PointF(0,0)));

                ms = new MemoryStream();
                
                img.Save(ms, new JpegEncoder());
                
                var dirPath = Path.GetFullPath("wwwroot/test");
            
                var path1 = Path.Combine(dirPath, Guid.NewGuid().ToString()+".jpg");
                
                img.Save(path1);
            }

            MemoryCache ms;
            ms.CreateEntry("",)
            
            ms.Seek(0, SeekOrigin.Begin);
            return File(ms, "image/jpeg");
        }*/
    }
}