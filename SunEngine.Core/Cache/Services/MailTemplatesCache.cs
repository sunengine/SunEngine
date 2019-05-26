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
        private const string MailTemplatesDir = "MailTemplates";
        
        
        private Dictionary<string, string> _templates;

        public Dictionary<string, string> Templates
        {
            get
            {
                if (_templates == null) 
                    Initialize();
                
                return _templates;
            }
        }
        

        // todo: add exception handling.
        public void Initialize()
        {
            _templates = new Dictionary<string, string>();

            DirectoryInfo directoryInfo = new DirectoryInfo(MailTemplatesDir);
            foreach (FileInfo file in directoryInfo.GetFiles())
            {
                string filePath = file.Name;
                string fileContent = File.ReadAllText(Path.Combine(MailTemplatesDir, file.Name));
                _templates.Add(filePath, fileContent);
            }
        }

        public MailContent BuildMessage(
            string templateName,
            Dictionary<string, string> replaceDictionary
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

            mailContent.subject = ParseHtmlValue(value, "Subject");
            mailContent.template = ParseHtmlValue(value, "Body");

            return mailContent;
        }

        public Task InitializeAsync()
        {
            throw new NotImplementedException();
        }

        public void Reset()
        {
            _templates = null;
        }

        private static string ParseHtmlValue(string rawString, string key)
        {
            var regex = new Regex($"<{key}>(.+)?<\\/{key}>", RegexOptions.Singleline);
            return regex.Match(rawString).Groups[1].Value.Trim();
        }
    }

    public class MailContent
    {
        public string subject = "null";
        public string template = "null";
    }
}
