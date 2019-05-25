using System.Collections.Generic;
using Xunit;

namespace SunEngine.Tests.SunEngine.Admin.Services
{
    public class MailTemplatesCache
    {
        [Fact]
        internal void Initialize()
        {
            Dictionary<string, string> exceptedDictionary = new Dictionary<string, string>();
            var mailTemplatesCache = new Core.Cache.Services.MailTemplatesCache();
            mailTemplatesCache.Initialize();
            exceptedDictionary.Add("test-template.html", "<head></head>\n");
            Assert.Equal(exceptedDictionary.ToString(), mailTemplatesCache.Templates.ToString());
        }
    }
}
