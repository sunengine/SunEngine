using System;
using System.IO;

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
    }
}