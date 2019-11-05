using System.IO;
using System.IO.Compression;
using System.Linq;
using Microsoft.Extensions.Hosting;
using SunEngine.Core.Errors;

namespace SunEngine.Admin.Services
{
    public class SkinsAdminService
    {
        protected readonly string AllSkinsPath;
        protected readonly string CurrentSkinPath;
        protected readonly string SkinNamePath;


        public SkinsAdminService(IHostEnvironment env)
        {
            var basePath = Path.Combine(env.ContentRootPath, "wwwroot", "statics");
            AllSkinsPath = Path.Combine(basePath, "skins");
            CurrentSkinPath = Path.Combine(basePath, "skin");
            SkinNamePath = Path.Combine(basePath, "skin", "name.txt");
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

            zipArchive.ExtractToDirectory(AllSkinsPath, true);
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
        }

        public object GetAllSkins()
        {
            var skinsPaths = Directory.GetDirectories(AllSkinsPath);
            var skins = skinsPaths.Select(Path.GetFileName).ToArray();

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