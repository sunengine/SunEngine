using System;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text.RegularExpressions;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using SunEngine.Core.Errors;

namespace SunEngine.Admin.Services
{
    public class SkinsAdminService
    {
        protected readonly string WwwRootPath;
        protected readonly string AllSkinsPath;
        protected readonly string CurrentSkinPath;
        protected readonly string SkinNamePath;
        protected readonly IWebHostEnvironment env;

        public SkinsAdminService(IWebHostEnvironment env)
        {
            this.env = env;
            WwwRootPath = Path.Combine(env.ContentRootPath, "wwwroot");
            var staticsPath = Path.Combine(WwwRootPath, "statics");
            AllSkinsPath = Path.Combine(staticsPath, "skins");
            CurrentSkinPath = Path.Combine(staticsPath, "skin");
            SkinNamePath = Path.Combine(staticsPath, "skin", "name.txt");
        }

        public void UploadSkin(string fileName, Stream fileStream)
        {
            var extension = Path.GetExtension(fileName);
            var name = Path.GetFileNameWithoutExtension(fileName);

            if (extension != ".zip")
                throw new SunViewException(new ErrorView("NotValidSkinFileNotZip", "Skin file has to be .zip",
                    ErrorType.System));

            var zipArchive = new ZipArchive(fileStream);

            // TODO need to check archive for security

            var skinDirPath = Path.Combine(AllSkinsPath, name);
            
            Directory.Delete(skinDirPath,true);
            Directory.CreateDirectory(skinDirPath);
            
            zipArchive.ExtractToDirectory(skinDirPath, true);
        }

        public void DeleteSkin(string name)
        {
            var pathToDelete = Path.Combine(AllSkinsPath, name);
            Directory.Delete(pathToDelete, true);
        }

        public void ChangeSkin(string name)
        {
            var selectedSkinPath = Path.Combine(AllSkinsPath, name);

            Directory.Delete(CurrentSkinPath, true);
            Directory.CreateDirectory(CurrentSkinPath);

            CopyDir(selectedSkinPath, CurrentSkinPath);

            System.IO.File.WriteAllText(SkinNamePath, name);

            if (env.IsProduction())
            {
                var ran = new Random();
                var configJsPath = Path.Combine(WwwRootPath, "config.js");
                var text = System.IO.File.ReadAllText(configJsPath);
                Regex reg1 = new Regex("skinver=\\d+\"");
                text = reg1.Replace(text, $"skinver={ran.Next()}\"");
                System.IO.File.WriteAllText(configJsPath, text);
                
                var indexHtmlPath = Path.Combine(WwwRootPath, "index.html");
                text = System.IO.File.ReadAllText(indexHtmlPath);
                Regex reg2 = new Regex("configver=\\d+\"");
                text = reg2.Replace(text, $" configver={ran.Next()}\"");
                System.IO.File.WriteAllText(indexHtmlPath, text);
            }
        }

        public object GetAllSkins()
        {
            var skinsPaths = Directory.GetDirectories(AllSkinsPath);
            var skins = skinsPaths.Select(Path.GetFileName).OrderBy(x=>x).ToArray();

            var nameFilePath = Path.Combine(CurrentSkinPath, "name.txt");
            var currentSkin = System.IO.File.ReadAllText(nameFilePath);

            return new {current = currentSkin, all = skins};
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