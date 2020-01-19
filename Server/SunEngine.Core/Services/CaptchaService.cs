using System.IO;
using System.Linq;
using SixLabors.Fonts;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Formats.Jpeg;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing;
using SunEngine.Core.Cache.Services;
using SunEngine.Core.Utils;
using Path = System.IO.Path;
using PointF = SixLabors.Primitives.PointF;

namespace SunEngine.Core.Services
{
	public class CaptchaService
	{
		public const string CypherName = "Captcha";

		private readonly Font font;
		private readonly CaptchaCacheService captchaCacheService;

		public CaptchaService(
			IPathService pathService,
			CaptchaCacheService captchaCacheService)
		{
			this.captchaCacheService = captchaCacheService;
			// Init Font (font name: Gunny Rewritten)
			FontCollection fontCollection = new FontCollection();
			var resourcesDir = pathService.GetPath(PathNames.ResourcesDirName);
			fontCollection.Install(Path.Combine(resourcesDir, "gunnyrewritten.ttf"));
			font = fontCollection.Families.First().CreateFont(46);
		}

		public string MakeCaptchaToken()
		{
			var token = CryptoRandomizer.GetRandomString(64);
			var answer = GenerateCaptchaText();

			captchaCacheService.Cache(token, answer);
			return token;
		}

		private string GenerateCaptchaText()
		{
			return CryptoRandomizer.GetRandomInt(10000, 999999).ToString();
		}

		public string GetAnswerByToken(string token)
		{
			return captchaCacheService.GetCaptchaAnswer(token);
		}

		public bool VerifyToken(string token, string text)
		{
			if (string.IsNullOrEmpty(token) ||
			    string.IsNullOrEmpty(text))
				return false;

			var answer = captchaCacheService.GetCaptchaAnswer(token);
			if (string.IsNullOrEmpty(answer))
				return false;

			captchaCacheService.InvalidateToken(token);
			return string.Equals(answer, text);
		}

		public MemoryStream MakeCaptchaImage(string text)
		{
			RendererOptions ro = new RendererOptions(font)
			{
				VerticalAlignment = VerticalAlignment.Center,
				TabWidth = 10
			};

			var rect = TextMeasurer.MeasureBounds(text, ro);

			MemoryStream ms;
			using (Image<Rgba32> img = new Image<Rgba32>((int) rect.Width + 10, (int) rect.Height + 6))
			{
				var textGraphicsOptions =
					new TextGraphicsOptions(true)
					{
						VerticalAlignment = VerticalAlignment.Center,
						TabWidth = 10
					};

				PointF[] points = {new PointF(2, img.Height / 2), new PointF(img.Width - 2, img.Height / 2)};

				img.Mutate(ctx => ctx
					.Fill(Rgba32.FromHex("f0f4c3")) // white background image
					.DrawLines(Rgba32.Black, 3, points)
					.DrawText(textGraphicsOptions, text, font, Rgba32.Black, new PointF(0, img.Height / 2)));

				ms = new MemoryStream();

				img.Save(ms, new JpegEncoder());
			}

			ms.Seek(0, SeekOrigin.Begin);
			return ms;
		}
	}
}