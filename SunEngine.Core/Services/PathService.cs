using System.Text.RegularExpressions;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Options;
using SunEngine.Core.Configuration.Options;

namespace SunEngine.Core.Services
{
    public interface IPathService
    {
        string MakePath(string token);
    }

    public class PathService : IPathService
    {
        public readonly string WwwRootDir;
        public readonly string ApplicationDir;
        
        
        public PathService(
            IOptions<GlobalOptions> globalOptions,
            IWebHostEnvironment env)
        {
            ApplicationDir = env.ContentRootPath;
            WwwRootDir = MakePath(globalOptions.Value.WwwRootDir);
        }
        
        public string MakePath(string token)
        {
            if (token.StartsWith("~"))
                return System.IO.Path.Combine(ApplicationDir, token.Substring(1));
            if(token.StartsWith("@"))
                return System.IO.Path.Combine(WwwRootDir, token.Substring(1));
            
            return token;
        }
    }
    
    
}