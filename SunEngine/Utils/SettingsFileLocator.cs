using System;
using System.IO;
using Microsoft.AspNetCore.Hosting;

namespace SunEngine.Utils
{
    public static class SettingsFileLocator
    {
        public static string GetSettingFilePath(string fileName, bool optional = false)
        {
            string fileLocal = "local." + fileName;
            string pathLocal =  Path.GetFullPath(fileLocal);
            if (File.Exists(pathLocal))
                return pathLocal;

            string fileCommon = fileName;
            string pathCommon = Path.GetFullPath(fileCommon);
            if (File.Exists(pathCommon))
                return pathCommon;

            if (!optional)
                throw new Exception($"Can not locate local.{fileName} or {fileName}");

            return null;
        }
        
        public static string GetEnvSuffix(IHostingEnvironment env)
        {
            if (env.IsDevelopment()) return "dev";
            if (env.IsProduction()) return "prod";
            return env.EnvironmentName.ToLower();
        }
    }
}