using System;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace SunEngine.Core.Cache.Services
{
    public class MailTemplatesCache : ISunMemoryCache
    {
        protected const string MailTemplatesDir = "MailTemplates";


        protected Dictionary<string, MailContent> templates;

        protected string layout;


        public MailContent BuildMessage(string templateName, Dictionary<string, string> replaceDictionary)
        {
            if (!templates.ContainsKey(templateName))
                throw new Exception($"Mail template {templateName} not found");

            string subject = templates[templateName].subject;
            string body = templates[templateName].template;

            foreach (var (key, value) in replaceDictionary)
            {
                subject = subject.Replace(key, value);
                body = body.Replace(key, value);
            }

            body = layout.Replace("[content]", body);

            MailContent mailContent = new MailContent
            {
                subject = subject, 
                template = body
            };

            return mailContent;
        }

        // TODO: add exception handling.
        public void Initialize()
        {
            templates = new Dictionary<string, MailContent>();

            DirectoryInfo directoryInfo = new DirectoryInfo(MailTemplatesDir);
            foreach (FileInfo file in directoryInfo.GetFiles())
            {
                string fileContent = File.ReadAllText(Path.Combine(MailTemplatesDir, file.Name));
                
                if (file.Name == "layout.html")
                {
                    layout = fileContent;
                    continue;
                }

                MailContent mailContent = new MailContent
                {
                    subject = ParseHtmlValue(fileContent, "Subject"),
                    template = ParseHtmlValue(fileContent, "Body")
                };

                templates.Add(file.Name, mailContent);
            }
        }

        private static string ParseHtmlValue(string rawString, string key)
        {
            var regex = new Regex($"<{key}>(.+)?<\\/{key}>", RegexOptions.Singleline);
            return regex.Match(rawString).Groups[1].Value.Trim();
        }

        public Task InitializeAsync()
        {
            throw new NotImplementedException();
        }

        public void Reset()
        {
            templates = null;
            layout = null;
        }
    }

    public class MailContent
    {
        public string subject = "null";
        public string template = "null";
    }
}
