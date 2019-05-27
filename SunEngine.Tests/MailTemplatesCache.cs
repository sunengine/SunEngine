using System.Collections.Generic;
using Xunit;
using Xunit.Abstractions;

namespace SunEngine.Tests
{
    public class MailTemplatesCache
    {
        private readonly ITestOutputHelper testOutputHelper;

        public MailTemplatesCache(ITestOutputHelper testOutputHelper)
        {
            this.testOutputHelper = testOutputHelper;
        }

        private readonly Dictionary<string, string> exceptedDictionary =
            new Dictionary<string, string>();

        private readonly Core.Cache.Services.MailTemplatesCache mailTemplatesCache =
            new Core.Cache.Services.MailTemplatesCache();

        [Fact]
        internal void FileContentEqualExistsTest()
        {
            exceptedDictionary.Add("test-template.html", "<head></head>\n");
            Assert.Equal(
                exceptedDictionary.ToString(),
                mailTemplatesCache.Templates.ToString()
            );
        }

        [Fact]
        internal void BuildMessageSubjectTest()
        {
            Assert.Equal(
                "Please confirm your account",
                mailTemplatesCache.BuildMessage(
                    "test-register.html",
                    new Dictionary<string, string> {{"space", ""}}
                ).subject
            );
        }

        [Fact]
        internal void BuildMessageBodyTest()
        {
            Assert.Equal(
                "Please confirm your account by clicking this <a href=\"google.com\">link</a>.",
                mailTemplatesCache.BuildMessage(
                    "test-register.html",
                    new Dictionary<string, string> {{"[link]", "google.com"}}
                ).template
            );
        }
    }
}
