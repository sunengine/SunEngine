using System.Collections.Generic;
using System.IO;
using System.Linq;
using Microsoft.Extensions.Configuration;
using SunEngine.Core.Errors.Exceptions;
using Path = System.IO.Path;

namespace SunEngine.Core.Services
{
    public interface IPathService
    {
        string MakePath(string token);
        string GetPath(string dirName);
        string Combine(string dirName, params string[] args);

        string WwwRootDir { get; }
        string ApplicationDir { get; }
        string ConfigDir { get; }
    }

    public static class PathNames
    {
        public const string ApplicationRootName = "ApplicationRoot";
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

        public PathService(IConfiguration configuration)
        {
            Pathes = new Dictionary<string, string>();

            var dirsSection = configuration.GetSection("Dirs");
            var dic = dirsSection.GetChildren().ToDictionary(x => x.Key, x => x.Value);

            if (!dic.TryGetValue(PathNames.ApplicationRootName, out string applicationRootToken))
                throw new SunException($"No {PathNames.ApplicationRootName} in config Global section");
            if (applicationRootToken == "auto")
                ApplicationDir = SearchApplicationRootDir();
            else
                ApplicationDir = applicationRootToken;

            if (!dic.TryGetValue(PathNames.WwwRootDirName, out string wwwRootDirToken))
                throw new SunException($"No {PathNames.WwwRootDirName} in config Global section");
            WwwRootDir = MakePath(wwwRootDirToken);

            if (!dic.TryGetValue(PathNames.ConfigDirName, out string configToken))
                throw new SunException($"No {PathNames.ConfigDirName} in config Global section");
            ConfigDir = MakePath(configToken);

            foreach (var (key, value) in dic)
                Pathes.Add(key, MakePath(value));
        }

        public static string SearchApplicationRootDir()
        {
            List<string> dirTokens = Directory.GetCurrentDirectory()
                .Split(Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar).ToList();
            for (int i = 0; i < 3; i++)
            {
                var currentPath = string.Join(Path.DirectorySeparatorChar, dirTokens);
                if (CheckDir(currentPath))
                    return currentPath;

                if (dirTokens.Count >= 2)
                    dirTokens.RemoveAt(dirTokens.Count - 1);
                else
                    break;
            }

            throw new SunException("Can not find Application root directory");

            bool CheckDir(string path) => File.Exists(Path.Combine(path, "Data", "Resources", "Roles.schema.json"));
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
                return Combine(ApplicationDir, Split(token.Substring(AppDirPrefix.Length + 1)));
            if (token.StartsWith(WwwRootDirPrefix))
                return Combine(WwwRootDir, Split(token.Substring(WwwRootDirPrefix.Length + 1)));
            if (token.StartsWith(ConfigDirPrefix))
                return Combine(ConfigDir, Split(token.Substring(ConfigDirPrefix.Length + 1)));

            return token;

            string[] Split(string tkn) => tkn.Split('\\', '/');
            string Combine(string start, string[] next) => System.IO.Path.Combine(start, System.IO.Path.Combine(next));
        }
    }
}