using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Options;
using SunEngine.Core.Configuration.Options;

namespace SunEngine.Core.Services
{
    public interface IPathService
    {
        string MakePath(string token);
        string WwwRootDir { get; }
        string ApplicationDir { get; }
        string ConfigDir { get; }
    }

    public class PathService : IPathService
    {
        public string WwwRootDir { get; protected set; }
        public string ApplicationDir { get; protected set; }
        public string ConfigDir { get; protected set; }

        public PathService(
            IOptionsMonitor<GlobalOptions> globalOptions,
            IHostingEnvironment env)
        {
            ApplicationDir = env.ContentRootPath;
            WwwRootDir = MakePath(globalOptions.CurrentValue.WwwRootDir);
            ConfigDir = globalOptions.CurrentValue.ConfigRootDir;
        }

        private const string WwwRootDirPrefix = "%wwwroot%";
        private const string AppDirPrefix = "%app%";
        private const string ConfigDirPrefix = "%config%";

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