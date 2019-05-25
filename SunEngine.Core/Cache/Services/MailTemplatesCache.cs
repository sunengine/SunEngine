using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace SunEngine.Core.Cache.Services
{
    public class MailTemplatesCache : ISunMemoryCache
    {
        public Dictionary<string, string> Templates { get; } = new Dictionary<string, string>();

        // todo: add exception handling.
        public void Initialize()
        {
            DirectoryInfo directoryInfo = new DirectoryInfo("MailTemplates");
            foreach (FileInfo file in directoryInfo.GetFiles())
            {
                string filePath = file.Name;
                string fileContent = File.ReadAllText($"MailTemplates\\{file.Name}");
                Templates.Add(filePath, fileContent);
            }
        }

        public Task InitializeAsync() => throw new NotImplementedException();

        public void Reset() => throw new NotImplementedException();
    }
}
