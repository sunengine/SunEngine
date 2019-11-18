using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text.RegularExpressions;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using SunEngine.Core.Configuration.Options;
using SunEngine.Core.Errors;
using SunEngine.Core.Errors.Exceptions;
using SunEngine.Core.Services;
using SunEngine.Core.Utils;
using IHostingEnvironment = Microsoft.AspNetCore.Hosting.IHostingEnvironment;

namespace SunEngine.Admin.Services
{
    public class SkinsAdminService
    {
        public readonly string WwwRootPath;
        public readonly string AllSkinsPath;
        public readonly string CurrentSkinPath;
        public readonly IHostingEnvironment env;

        private readonly int MaxArchiveSize;
        private readonly int MaxExtractArchiveSize;

        private readonly List<string> requiredFiles = new List<string>()
        {
            "styles.css",
            "preview.png",
            "readme.md"
        };

        private readonly List<string> allowedExtensions = new List<string>()
        {
            ".scss", ".sass", ".css", ".map", ".png", ".jpg", ".jpeg", ".gif",
            ".svg", ".woff", ".woff2", ".ttf", ".otf", ".json", ".md"
        };

        public SkinsAdminService(
            IPathService pathService,
            IOptionsMonitor<SkinsOptions> skinsOptions,
            IOptionsMonitor<FileLoadingOptions> fileLoadingOptions,
            IHostingEnvironment env)
        {
            this.env = env;

            AllSkinsPath = pathService.MakePath(skinsOptions.CurrentValue.AllSkinsDir);
            CurrentSkinPath = pathService.MakePath(skinsOptions.CurrentValue.CurrentSkinDir);
            MaxArchiveSize = fileLoadingOptions.CurrentValue.MaxArchiveSize * 1024;
            MaxExtractArchiveSize = fileLoadingOptions.CurrentValue.MaxExtractArchiveSize * 1024;
            WwwRootPath = pathService.WwwRootDir;
        }

        public void UploadSkin(IFormFile file)
        {
            var fileName = file.FileName;
            var fileStream = file.OpenReadStream();
            var extension = Path.GetExtension(fileName);

            if (extension != ".zip")
                throw new SunErrorException(new Error("NotValidSkinFileNotZip", "Skin file has to be .zip",
                    ErrorType.System));

            if (file.Length > MaxArchiveSize)
                throw new SunErrorException(new Error("VeryBigFile", $"Max file size {MaxArchiveSize}Kb",
                    ErrorType.System));

            var zipArchive = new ZipArchive(fileStream);
            var zipEntry = zipArchive.GetEntry("info.json");
            if (zipEntry == null)
                throw new SunErrorException(new Error("SkinFileNotContainInfoJson",
                    "Skin archive do not contain info.json file",
                    ErrorType.System));

            if (zipArchive.Entries.Sum(entry => entry.Length) > MaxExtractArchiveSize)
            {
                throw new SunErrorException(new Error("VeryBigExtractArchive",
                    $"Max extract archive size {MaxExtractArchiveSize}Kb",
                    ErrorType.System));
            }

            var fileNames = zipArchive.Entries.Select(x => x.Name);
            var missingFiles = requiredFiles.Where(x => !fileNames.Contains(x)).ToList();
            if (missingFiles.Count > 0)
            {
                var strMissingFiles = missingFiles.Aggregate((x, y) => $"{x}, {y}");
                throw new SunErrorException(new Error("MissingRequiredFiles",
                    $"Missing required files: {strMissingFiles}"));
            }

            var hasDisallowedFile =
                zipArchive.Entries.All(entry => allowedExtensions.Contains(Path.GetExtension(entry.FullName)));
            if (!hasDisallowedFile)
                throw new SunErrorException(new Error("HasDisallowedFile", ""));

            var jsonString = new StreamReader(zipEntry.Open()).ReadToEnd();
            var skinInfo = JsonConvert.DeserializeObject<SkinInfo>(jsonString);
            var skinDirPath = Path.Combine(AllSkinsPath, PathUtils.ClearPathToken(skinInfo.Name));

            if (Directory.Exists(skinDirPath))
                Directory.Delete(skinDirPath, true);
            Directory.CreateDirectory(skinDirPath);

            zipArchive.ExtractToDirectory(skinDirPath, true);
        }

        public void DeleteSkin(string name)
        {
            var secureSkinName = PathUtils.ClearPathToken(name);
            var pathToDelete = Path.Combine(AllSkinsPath, secureSkinName);
            Directory.Delete(pathToDelete, true);
        }

        public void ChangeSkin(string name)
        {
            var secureSkinName = PathUtils.ClearPathToken(name);

            var selectedSkinPath = Path.Combine(AllSkinsPath, secureSkinName);

            if (Directory.Exists(CurrentSkinPath))
                Directory.Delete(CurrentSkinPath, true);
            Directory.CreateDirectory(CurrentSkinPath);

            CopyDir(selectedSkinPath, CurrentSkinPath);

            if (env.IsProduction())
            {
                var ran = new Random();
                var configJsPath = Path.Combine(WwwRootPath, "variables.js");
                var text = File.ReadAllText(configJsPath);
                Regex reg1 = new Regex("skinver=\\d+\"");
                text = reg1.Replace(text, $"skinver={ran.Next()}\"");
                File.WriteAllText(configJsPath,text);
                
                var indexHtmlPath = Path.Combine(WwwRootPath, "index.html");
                text = File.ReadAllText(indexHtmlPath);
                Regex reg2 = new Regex("variablesver=\\d+\"");
                text = reg2.Replace(text, $"variablesver={ran.Next()}\"");
                File.WriteAllText(indexHtmlPath,text);
            }
        }

        public List<SkinInfo> GetAllSkins()
        {
            var skinsPaths = Directory.GetDirectories(AllSkinsPath);
            var skins = skinsPaths.Select(Path.GetFileName).OrderBy(x => x).ToArray();

            var currentSkinInfoJsonPath = Path.Combine(CurrentSkinPath, "info.json");

            string currentSkinName = null;

            if (File.Exists(currentSkinInfoJsonPath))
            {
                try
                {
                    SkinInfo currentSkinInfo = JsonConvert.DeserializeObject<SkinInfo>(
                        File.ReadAllText(currentSkinInfoJsonPath));
                    currentSkinName = currentSkinInfo.Name;
                }
                catch
                {
                    // ignored
                }
            }


            var skinsInfos = new List<SkinInfo>();

            foreach (var skin in skins)
            {
                try
                {
                    var jsonInfo = System.IO.File.ReadAllText(Path.Combine(AllSkinsPath, skin, "info.json"));
                    SkinInfo skinInfo = JsonConvert.DeserializeObject<SkinInfo>(jsonInfo);
                    if (skinInfo.Name == currentSkinName)
                        skinInfo.Current = true;

                    skinsInfos.Add(skinInfo);
                }
                catch (Exception)
                {
                    // ignored
                }
            }

            return skinsInfos;
        }

        public class SkinInfo
        {
            public string Name { get; set; }
            public string Author { get; set; }
            public string[] Contacts { get; set; }
            public int Version { get; set; }
            public string SourceUrl { get; set; }
            public string Description { get; set; }
            public bool Current { get; set; }
        }


        protected void CopyDir(string fromPath, string toPath)
        {
            foreach (string dirPath in Directory.GetDirectories(fromPath, "*",
                SearchOption.AllDirectories))
                Directory.CreateDirectory(dirPath.Replace(fromPath, toPath));

            foreach (string newPath in Directory.GetFiles(fromPath, "*.*",
                SearchOption.AllDirectories))
                System.IO.File.Copy(newPath, newPath.Replace(fromPath, toPath), true);
        }
    }
}