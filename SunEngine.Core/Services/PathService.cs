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

        public string MakePath(string token)
        {
            if (token.StartsWith("~"))
                return System.IO.Path.Combine(ApplicationDir, token.Substring(1));
            if (token.StartsWith("@"))
                return System.IO.Path.Combine(WwwRootDir, token.Substring(1));

            return token;
        }
    }
}