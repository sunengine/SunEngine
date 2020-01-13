using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;
using Microsoft.Extensions.Options;
using SunEngine.Core.Configuration.Options;
using SunEngine.Core.Errors.Exceptions;
using SunEngine.Core.Services;

namespace SunEngine.Core.Cache.Services
{
	public interface IMailTemplatesCache
	{
		MailContent BuildMessage(string templateName, Dictionary<string, string> replaceDictionary);
		void Initialize();
	}

	public class MailTemplatesCache : IMailTemplatesCache
	{
		protected readonly string SiteName;
		protected readonly string MailTemplatesDir;

		public Dictionary<string, MailContent> Templates { get; protected set; }
		public string Layout { get; protected set; }

		public MailTemplatesCache(IPathService pathService, IOptionsMonitor<GlobalOptions> globalOptions)
		{
			MailTemplatesDir = pathService.GetPath(PathNames.MailTemplatesDirName);
			if (!Directory.Exists(MailTemplatesDir))
				Directory.CreateDirectory(MailTemplatesDir);
			SiteName = globalOptions.CurrentValue.SiteName;
			Initialize();
		}

		public MailContent BuildMessage(string templateName, Dictionary<string, string> replaceDictionary)
		{
			if (!Templates.ContainsKey(templateName))
				throw new SunException($"Mail template {templateName} not found");

			string subject = Templates[templateName].Subject;
			string body = Templates[templateName].Template;

			foreach (var (key, value) in replaceDictionary)
			{
				subject = subject.Replace(key, value);
				body = body.Replace(key, value);
			}

			body = Layout.Replace("[content]", body);

			MailContent mailContent = new MailContent
			{
				Subject = subject,
				Template = body
			};

			return mailContent;
		}

		public void Initialize()
		{
			Templates = new Dictionary<string, MailContent>();

			DirectoryInfo directoryInfo = new DirectoryInfo(MailTemplatesDir);
			foreach (FileInfo file in directoryInfo.GetFiles())
			{
				string fileContent = File.ReadAllText(Path.Combine(MailTemplatesDir, file.Name));

				if (file.Name == "layout.html")
				{
					Layout = fileContent.Replace("[sitename]", SiteName);
					continue;
				}

				MailContent mailContent = new MailContent
				{
					Subject = ParseHtmlValue(fileContent, "Subject"),
					Template = ParseHtmlValue(fileContent, "Body")
				};

				Templates.Add(file.Name, mailContent);
			}

			string ParseHtmlValue(string rawString, string key)
			{
				var regex = new Regex($"<{key}>(.+)?<\\/{key}>", RegexOptions.Singleline);
				return regex.Match(rawString).Groups[1].Value.Trim();
			}
		}
	}

	public class MailContent
	{
		public string Subject = "null";
		public string Template = "null";
	}
}