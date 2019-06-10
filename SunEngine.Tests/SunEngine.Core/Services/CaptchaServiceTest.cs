using System.IO;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using SunEngine.Core.Configuration.Options;
using SunEngine.Core.Services;
using Xunit;

namespace SunEngine.Tests.SunEngine.Core.Services
{
    public class CaptchaServiceTest
    {
        private ServiceProvider serviceProvider;
        private CaptchaService captchaService;

        public CaptchaServiceTest()
        {
            serviceProvider = DefaultInit.DefaultServiceProvider();
            IOptions<CaptchaOptions> captchaOptions = serviceProvider.GetRequiredService<IOptions<CaptchaOptions>>();
            var cryptService = serviceProvider.GetRequiredService<CryptService>();

            captchaService = new CaptchaService(captchaOptions, cryptService);
        }

        [Fact]
        public void ShouldMakeCryptedCaptchaToken()
        {
            string token = captchaService.MakeCryptedCaptchaToken();

            Assert.NotNull(token);
            Assert.NotEqual(string.Empty, token);
        }

        [Fact]
        public void ShouldReturnTextFromToken()
        {
            string token = captchaService.MakeCryptedCaptchaToken();
            string textFromToken = captchaService.GetTextFromToken(token);

            Assert.NotNull(textFromToken);
            Assert.NotEqual(string.Empty, textFromToken);
        }

        [Fact]
        public void ShouldReturnTrueOnVerifyToken()
        {
            string token = captchaService.MakeCryptedCaptchaToken();
            string textFromToken = captchaService.GetTextFromToken(token);
            
            Assert.True(captchaService.VerifyToken(token, textFromToken));
        }

        [Fact]
        public void ShouldReturnFalseOnVerifyTokenWithInvalidText()
        {
            string token = captchaService.MakeCryptedCaptchaToken();
            string textFromToken = string.Empty;

            Assert.False(captchaService.VerifyToken(token, textFromToken));
        }

        [Fact]
        public void ShouldReturnFalseOnVerifyTokenWithInvalidToken()
        {
            string token = captchaService.MakeCryptedCaptchaToken();
            string textFromToken = captchaService.GetTextFromToken(token);

            Assert.False(captchaService.VerifyToken(captchaService.MakeCryptedCaptchaToken(), textFromToken));
        }

        [Fact]
        public void ShouldMakeCaptchaImage()
        {
            string text = "fghjt4Tjg";
            MemoryStream ms = captchaService.MakeCaptchaImage(text);
            
            Assert.NotNull(ms);
            ms.Dispose();
        }
    }
}
