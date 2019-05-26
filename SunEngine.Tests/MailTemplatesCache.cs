using System.Collections.Generic;
using System.Diagnostics;
using Xunit;
using Xunit.Abstractions;

namespace SunEngine.Tests
{
    public class MailTemplatesCache
    {
        private readonly ITestOutputHelper testOutputHelper;

        private readonly Dictionary<string, string> exceptedDictionary =
            new Dictionary<string, string>();

        private readonly Core.Cache.Services.MailTemplatesCache mailTemplatesCache =
            new Core.Cache.Services.MailTemplatesCache();

        public MailTemplatesCache(ITestOutputHelper testOutputHelper)
        {
            this.testOutputHelper = testOutputHelper;
        }

        [Fact]
        internal void FileDictionaryExistsTest()
        {
            mailTemplatesCache.Initialize();
            exceptedDictionary.Add("test-template.html", "<head></head>\n");
            Assert.Equal(
                exceptedDictionary.ToString(),
                mailTemplatesCache.Templates.ToString()
            );
        }

//        [Fact]
//        internal void KeyValueExistsTest()
//        {
//            mailTemplatesCache.Initialize();
//            Assert.Equal(
//                "<head></head>\r\n",
//                mailTemplatesCache.BuildMessage(
//                    "test-template.html",
//                    null
//                )
//            );
//        }
        [Fact]
        internal void ParseStringTest()
        {
            var s = new Dictionary<string, string> {{"link", "https://google.com/"}};
            var ss = mailTemplatesCache.BuildMessage("register.html", s);
            Debug.WriteLine(ss.subject);
            Debug.WriteLine(ss.template);
        }
    }
}
