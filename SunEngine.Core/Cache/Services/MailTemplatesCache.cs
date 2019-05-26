using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace SunEngine.Core.Cache.Services
{
    public class MailTemplatesCache : ISunMemoryCache
    {
        private Dictionary<string, string> templates;

        public Dictionary<string, string> Templates
        {
            get
            {
                if (templates == null) Initialize();
                return templates;
            }
        }

        // todo: add exception handling.
        public void Initialize()
        {
            templates = new Dictionary<string, string>();

            DirectoryInfo directoryInfo = new DirectoryInfo("MailTemplates");
            foreach (FileInfo file in directoryInfo.GetFiles())
            {
                string filePath = file.Name;
                string fileContent = File.ReadAllText($"MailTemplates\\{file.Name}");
                templates.Add(filePath, fileContent);
            }
        }

        public MailContent BuildMessage(
            string templateName,
            Dictionary<String, String> replaceDictionary
        )
        {
            MailContent mailContent = new MailContent();

            if (!Templates.Keys.Contains(templateName))
            {
                return mailContent;
            }

            var value = Templates[templateName];
            for (int i = 0; i < replaceDictionary.Count; i++)
            {
                value = value.Replace(
                    replaceDictionary.Keys.ElementAt(i),
                    replaceDictionary[
                        replaceDictionary.Keys.ElementAt(i)
                    ]
                );
            }

            mailContent.subject = TrimHtmlTags(
                ParseHtmlValue(value, "Subject"),
                "Subject"
            );
            mailContent.template = TrimHtmlTags(
                ParseHtmlValue(value, "Body"),
                "Body"
            );

            return mailContent;
        }

        public Task InitializeAsync() => throw new NotImplementedException();

        public void Reset() => throw new NotImplementedException();

        private static string TrimHtmlTags(string matchedString, string key)
        {
            return matchedString.Trim($"</{key}>".ToCharArray()).Trim();
        }

        private static string ParseHtmlValue(string rawString, string key)
        {
            var regex = new Regex($"<{key}>(.+)?<\\/{key}>", RegexOptions.Singleline);
            return regex.Match(rawString).Value;
        }
    }

    public class MailContent
    {
        public string subject = "null";
        public string template = "null";
    }
}
