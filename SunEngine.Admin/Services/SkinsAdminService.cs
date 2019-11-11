using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text.Json.Serialization;
using System.Text.RegularExpressions;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using SunEngine.Core.Configuration.Options;
using SunEngine.Core.Errors;
using SunEngine.Core.Services;
using SunEngine.Core.Utils;

namespace SunEngine.Admin.Services
{
    public class SkinsAdminService
    {
        public readonly string WwwRootPath;
        public readonly string AllSkinsPath;
        public readonly string CurrentSkinPath;
        public readonly IWebHostEnvironment env;

        public SkinsAdminService(
            IPathService pathService,
            IOptionsMonitor<SkinsOptions> skinsOptions,
            IWebHostEnvironment env)
        {
            this.env = env;

            AllSkinsPath = pathService.MakePath(skinsOptions.CurrentValue.AllSkinsDir);
            CurrentSkinPath = pathService.MakePath(skinsOptions.CurrentValue.CurrentSkinDir);
        }

        public void UploadSkin(string fileName, Stream fileStream)
        {
            var extension = Path.GetExtension(fileName);

            if (extension != ".zip")
                throw new SunViewException(new ErrorView("NotValidSkinFileNotZip", "Skin file has to be .zip",
                    ErrorType.System));

            var zipArchive = new ZipArchive(fileStream);
            var zipEntry = zipArchive.GetEntry("info.json");
            if (zipEntry == null)
                throw new SunViewException(new ErrorView("SkinFileNotContainInfoJson",
                    "Skin archive do not contain info.json file",
                    ErrorType.System));

            var jsonString = new StreamReader(zipEntry.Open()).ReadToEnd();

            SkinInfo skinInfo = JsonConvert.DeserializeObject<SkinInfo>(jsonString);

            // TODO need to check archive for security

            var skinDirPath = Path.Combine(AllSkinsPath, skinInfo.Name);

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
                var configJsPath = Path.Combine(WwwRootPath, "config.js");
                var text = File.ReadAllText(configJsPath);
                Regex reg1 = new Regex("skinver=\\d+\"");
                text = reg1.Replace(text, $"skinver={ran.Next()}\"");

                var indexHtmlPath = Path.Combine(WwwRootPath, "index.html");
                text = File.ReadAllText(indexHtmlPath);
                Regex reg2 = new Regex("configver=\\d+\"");
                text = reg2.Replace(text, $" configver={ran.Next()}\"");
            }
        }

        public List<SkinInfo> GetAllSkins()
        {
            var skinsPaths = Directory.GetDirectories(AllSkinsPath);
            var skins = skinsPaths.Select(Path.GetFileName).OrderBy(x => x).ToArray();

            SkinInfo currentSkinInfo = JsonConvert.DeserializeObject<SkinInfo>(
                File.ReadAllText(Path.Combine(CurrentSkinPath, "info.json")));
            

            var skinsInfos = new List<SkinInfo>();

            foreach (var skin in skins)
            {
                try
                {
                    var jsonInfo = System.IO.File.ReadAllText(Path.Combine(AllSkinsPath, skin, "info.json"));
                    SkinInfo skinInfo = JsonConvert.DeserializeObject<SkinInfo>(jsonInfo);
                    if (skinInfo.Name == currentSkinInfo.Name)
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