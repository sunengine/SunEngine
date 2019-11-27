using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using SixLabors.Shapes;
using SunEngine.Core.Configuration.Options;
using SunEngine.Core.Errors.Exceptions;

namespace SunEngine.Core.Services
{
    public interface IPathService
    {
        string MakePath(string token);
        string GetPath(string name);
        string Combine(string dirName, params string[] args);

        string WwwRootDir { get; }
        string ApplicationDir { get; }
        string ConfigDir { get; }
    }

    public static class PathNames
    {
        public const string WwwRootDirName = "WwwRoot";
        public const string ConfigDirName = "Config";
        public const string ResourcesDirName = "Resources";
        public const string AllSkinsDirName = "AllSkins";
        public const string CurrentSkinDirName = "CurrentSkin";
        public const string UploadImagesDirName = "UploadImages";
        public const string MailTemplatesDirName = "MailTemplates";
    }

    public class PathService : IPathService
    {
        public string WwwRootDir { get; }
        public string ApplicationDir { get; }
        public string ConfigDir { get; }

        protected Dictionary<string, string> Pathes;

        public PathService(
            IConfigurationRoot configuration,
            IHostingEnvironment env)
        {
            ApplicationDir = env.ContentRootPath;

            Pathes = new Dictionary<string, string>();

            var dirsSection = configuration.GetSection("Dirs");
            var dic = dirsSection.GetChildren().ToDictionary(x => x.Key, x => x.Value);

            WwwRootDir = MakePath(PathNames.WwwRootDirName);
            dic.Remove(PathNames.WwwRootDirName);
            ConfigDir = MakePath(PathNames.ConfigDirName);
            dic.Remove(PathNames.ConfigDirName);
            
            foreach (var (key, value) in dic)
                Pathes.Add(key, MakePath(value));
        }

        private const string WwwRootDirPrefix = "%wwwroot%";
        private const string AppDirPrefix = "%app%";
        private const string ConfigDirPrefix = "%config%";

        public string GetPath(string dirName)
        {
            if (Pathes.TryGetValue(dirName, out var value))
                return value;

            throw new SunException($"No directory {dirName} in SunEngine.json Dirs");
        }

        public string Combine(string dirName, params string[] args)
        {
            var dirPath = GetPath(dirName);
            return System.IO.Path.Combine(dirPath, System.IO.Path.Combine(args));
        }

        public string MakePath(string token)
        {
            if (token.StartsWith(AppDirPrefix))
                return System.IO.Path.Combine(ApplicationDir, token.Substring(AppDirPrefix.Length + 1));
            if (token.StartsWith(WwwRootDirPrefix))
                return System.IO.Path.Combine(WwwRootDir, token.Substring(WwwRootDirPrefix.Length + 1));
            if (token.StartsWith(ConfigDirPrefix))
                return System.IO.Path.Combine(ConfigDir, token.Substring(ConfigDirPrefix.Length + 1));

            return token;
        }
    }
}