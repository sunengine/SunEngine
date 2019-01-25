using System;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using SixLabors.Fonts;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Formats.Jpeg;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing;
using SixLabors.Primitives;
using SixLabors.Shapes;
using SunEngine.Commons.Models;
using SunEngine.Options;
using Path = System.IO.Path;

namespace SunEngine.Services
{
    public class CaptchaToken
    {
        public string Text { get; set; }
        public DateTime Expire { get; set; }
        public Guid Guid { get; set; }
    }

    public class CaptchaService
    {
        private readonly TimeSpan cacheTimeout = new TimeSpan(0, 3, 0);

        public readonly byte[] SecurityKey;
        public readonly byte[] IV;


        private CryptService cryptService;

        public CaptchaService(IOptions<CaptchaOptions> captchaOptions, CryptService cryptService)
        {
            var numberOfBits = 256;

            SecurityKey = new byte[numberOfBits / 8]; // 8 bits per byte

            new RNGCryptoServiceProvider().GetBytes(SecurityKey);

            numberOfBits = 128;

            IV = new byte[numberOfBits / 8]; // 8 bits per byte

            new RNGCryptoServiceProvider().GetBytes(IV);


            this.cryptService = cryptService;
        }

        public string MakeCryptedCaptchaToken()
        {
            var token = new CaptchaToken
            {
                Text = GenerateCaptchaText(),
                Expire = DateTime.UtcNow.Add(cacheTimeout),
                Guid = Guid.NewGuid()
            };

            var tokenJson = JsonConvert.SerializeObject(token);
            return cryptService.Crypt(tokenJson, SecurityKey, IV);
        }


        string GenerateCaptchaText()
        {
            Random ran = new Random();
            string text = ran.Next(999999).ToString();
            return text;
        }

        public string GetTextFromToken(string token)
        {
            string json = cryptService.Decrypt(token, SecurityKey, IV);
            return JsonConvert.DeserializeObject<CaptchaToken>(json).Text;
        }

        public bool VerifyToken(string token, string text)
        {
            string json = cryptService.Decrypt(token, SecurityKey, IV);
            CaptchaToken captchaToken = JsonConvert.DeserializeObject<CaptchaToken>(json);
            if (captchaToken.Expire < DateTime.UtcNow)
                return false;

            return string.Equals(captchaToken.Text, text);
        }

        public MemoryStream MakeCaptchaImage(string text)
        {
            MemoryStream ms;
            using (Image<Rgba32> img = new Image<Rgba32>(400, 100))
            {
                PathBuilder pathBuilder = new PathBuilder();
                pathBuilder.SetOrigin(new PointF(500, 0));
                pathBuilder.AddBezier(new PointF(50, 450), new PointF(200, 50), new PointF(300, 50),
                    new PointF(450, 450));
                // add more complex paths and shapes here.

                IPath path = pathBuilder.Build();

                // For production application we would recomend you create a FontCollection
                // singleton and manually install the ttf fonts yourself as using SystemFonts
                // can be expensive and you risk font existing or not existing on a deployment
                // by deployment basis.

                var font = SystemFonts.Families.First()
                    .CreateFont(39); //.CreateFont("Arial", 39, FontStyle.Regular);

                //var font = SystemFonts.CreateFont("Arial", 39, FontStyle.Regular);

                var textGraphicsOptions =
                    new TextGraphicsOptions(true) // draw the text along the path wrapping at the end of the line
                    {
                        WrapTextWidth = path.Length
                    };
                img.Mutate(ctx => ctx
                    .Fill(Rgba32.White) // white background image
                    .Draw(Rgba32.Gray, 3,
                        path) // draw the path so we can see what the text is supposed to be following
                    .DrawText(textGraphicsOptions, text, font, Rgba32.Black, new PointF(0, 0)));

                ms = new MemoryStream();

                img.Save(ms, new JpegEncoder());

                //var dirPath = Path.GetFullPath("wwwroot/test");

                //var path1 = Path.Combine(dirPath, Guid.NewGuid().ToString() + ".jpg");

                //img.Save(path1);
            }


            ms.Seek(0, SeekOrigin.Begin);
            return ms;
        }
    }
}