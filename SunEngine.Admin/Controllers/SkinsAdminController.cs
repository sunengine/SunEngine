using System;
using System.IO;
using System.IO.Compression;
using System.Linq;
using LinqToDB.Metadata;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SunEngine.Core.Errors;

namespace SunEngine.Admin.Controllers
{
    public class SkinsAdminController : BaseAdminController
    {
        private readonly IWebHostEnvironment env;

        public SkinsAdminController(
            IWebHostEnvironment env,
            IServiceProvider serviceProvider) : base(serviceProvider)
        {
            this.env = env;
        }

        [HttpPost]
        public IActionResult UploadSkin(IFormFile file)
        {
            var extension = Path.GetExtension(file.FileName);
            var name = Path.GetFileNameWithoutExtension(file.FileName);

            if (extension != ".zip")
                throw new SunViewException(new ErrorView("NotValidSkinFileNotZip", "Skin file has to be .zip",
                    ErrorType.System));

            var zipArchive = new ZipArchive(file.OpenReadStream());

            var path = Path.Combine(env.ContentRootPath, "wwwroot", "statics", "skins");

            // TODO need to check archive for security

            zipArchive.ExtractToDirectory(path, true);

            return Ok();
        }

        [HttpPost]
        public IActionResult DeleteSkin(string name)
        {
            var selectedSkinPath = Path.Combine(env.ContentRootPath, "wwwroot", "statics", "skins", name);

            Directory.Delete(selectedSkinPath, true);

            return Ok();
        }

        [HttpPost]
        public IActionResult ChangeSkin(string name)
        {
            var currentSkinPath = Path.Combine(env.ContentRootPath, "wwwroot", "statics", "skin");
            var selectedSkinPath = Path.Combine(env.ContentRootPath, "wwwroot", "statics", "skins", name);

            Directory.Delete(currentSkinPath, true);
            Directory.CreateDirectory(currentSkinPath);

            //Now Create all of the directories
            foreach (string dirPath in Directory.GetDirectories(selectedSkinPath, "*",
                SearchOption.AllDirectories))
                Directory.CreateDirectory(dirPath.Replace(selectedSkinPath, currentSkinPath));

            //Copy all the files & Replaces any files with the same name
            foreach (string newPath in Directory.GetFiles(selectedSkinPath, "*.*",
                SearchOption.AllDirectories))
                System.IO.File.Copy(newPath, newPath.Replace(selectedSkinPath, currentSkinPath), true);

            var nameFilePath = Path.Combine(env.ContentRootPath, "wwwroot", "statics", "skin", "name.txt");

            System.IO.File.WriteAllText(nameFilePath, name);

            return Ok();
        }

        [HttpPost]
        public IActionResult GetAllSkins()
        {
            var path = Path.Combine(env.ContentRootPath, "wwwroot", "statics", "skins");
            var skinsPaths = Directory.GetDirectories(path);
            var skins = skinsPaths.Select(Path.GetFileName).ToArray();

            var nameFilePath = Path.Combine(env.ContentRootPath, "wwwroot", "statics", "skin", "name.txt");
            var currentSkin = System.IO.File.ReadAllText(nameFilePath);
            return Json(new {current = currentSkin, all = skins});
        }
    }
}