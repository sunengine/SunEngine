using System;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;

namespace SunEngine.Core.Cache.Services
{
    public interface IMailTemplatesCache : ISunMemoryCache
    {
        MailContent BuildMessage(string templateName, Dictionary<string, string> replaceDictionary);
    }

    public class MailTemplatesCache : IMailTemplatesCache
    {
        protected readonly object lockObject = new object();

        protected const string MailTemplatesDir = "MailTemplates";


        protected Dictionary<string, MailContent> _templates;
        protected string _layout;

        #region Getters

        protected Dictionary<string, MailContent> Templates
        {
            get
            {
                lock (lockObject)
                    if (_templates == null)
                        Initialize();

                return _templates;
            }
        }

        protected string Layout
        {
            get
            {
                lock (lockObject)
                    if (_layout == null)
                        Initialize();

                return _layout;
            }
        }

        #endregion

        public MailContent BuildMessage(string templateName, Dictionary<string, string> replaceDictionary)
        {
            if (!Templates.ContainsKey(templateName))
                throw new Exception($"Mail template {templateName} not found");

            string subject = Templates[templateName].subject;
            string body = Templates[templateName].template;

            foreach (var (key, value) in replaceDictionary)
            {
                subject = subject.Replace(key, value);
                body = body.Replace(key, value);
            }

            body = Layout.Replace("[content]", body);

            MailContent mailContent = new MailContent
            {
                subject = subject,
                template = body
            };

            return mailContent;
        }

        public void Initialize()
        {
            _templates = new Dictionary<string, MailContent>();

            DirectoryInfo directoryInfo = new DirectoryInfo(MailTemplatesDir);
            foreach (FileInfo file in directoryInfo.GetFiles())
            {
                string fileContent = File.ReadAllText(Path.Combine(MailTemplatesDir, file.Name));

                if (file.Name == "layout.html")
                {
                    _layout = fileContent;
                    continue;
                }

                MailContent mailContent = new MailContent
                {
                    subject = ParseHtmlValue(fileContent, "Subject"),
                    template = ParseHtmlValue(fileContent, "Body")
                };

                _templates.Add(file.Name, mailContent);
            }
            
            string ParseHtmlValue(string rawString, string key)
            {
                var regex = new Regex($"<{key}>(.+)?<\\/{key}>", RegexOptions.Singleline);
                return regex.Match(rawString).Groups[1].Value.Trim();
            }
        }


        public void Reset()
        {
            _templates = null;
            _layout = null;
        }
    }

    public class MailContent
    {
        public string subject = "null";
        public string template = "null";
    }
}
