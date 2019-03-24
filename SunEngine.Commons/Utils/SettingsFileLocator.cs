using System;
using System.IO;
using Microsoft.AspNetCore.Hosting;

namespace SunEngine.Commons.Utils
{
    public static class SettingsFileLocator
    {
        public static string GetSettingFilePath(string fileName, bool optional = false)
        {
            return GetSettingFilePath(null, fileName, optional);
        }
        
        public static string GetSettingFilePath(string dirName, string fileName, bool optional = false)
        {
            string fileLocal = "local." + fileName;
            string fileLocalWithDir = dirName != null ? Path.Combine(dirName, fileLocal) : fileLocal;
            
            string pathLocal = Path.GetFullPath(fileLocalWithDir);
            
            if (File.Exists(pathLocal))
                return pathLocal;

            string fileCommon = fileName;
            string fileCommonWithDir = dirName != null ? Path.Combine(dirName, fileCommon) : fileCommon;

            string pathCommon = Path.GetFullPath(fileCommonWithDir);
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