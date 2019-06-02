using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using LinqToDB;
using SunEngine.Core.DataBase;
using AngleSharp.Parser.Html;
using Flurl;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Options;
using SunEngine.Core.Configuration.Options;

namespace SunEngine.Admin.Services
{
    public class ImageCleanerAdminService
    {
        private readonly DataBaseConnection dataBaseConnection;
        private readonly string uploadDirectory;
        private readonly string uploadUrl;
        private readonly HtmlParser htmlParser;

        public ImageCleanerAdminService(
            DataBaseConnection dataBaseConnection,
            IHostingEnvironment env,
            IOptions<ImagesOptions> imagesOptions,
            IOptions<GlobalOptions> globalOptions)
        {
            this.dataBaseConnection = dataBaseConnection;
            uploadDirectory = Path.Combine(env.WebRootPath, imagesOptions.Value.UploadDir);
            uploadUrl = globalOptions.Value.SiteUrl.AppendPathSegment(imagesOptions.Value.UploadDir);
            htmlParser = new HtmlParser();
        }


        public async Task<List<string>> GetImageSourcesForCleanAsync()
        {
            List<string> sources = new List<string>();

            var imagesInDirectory = DirectoryExtensions.GetFilesWithExcludeChildDirectory(uploadDirectory, "*.*",
                SearchOption.AllDirectories, x => !x.StartsWith("_"));

            var materialSources = await GetMaterialSourcesFromASharp();

            var msgSources = await GetMessageSourcesFromASharp();
            materialSources.Union(msgSources);

            var avatarSources = await GetAvatarSources();
            materialSources.Union(avatarSources);

            foreach (var imagePath in imagesInDirectory)
            {
                var fileInfo = new FileInfo(imagePath);
                var path = Path.Combine(fileInfo.Directory.Name, fileInfo.Name);

                if (!materialSources.Contains(path))
                    sources.Add(path);
            }

            return sources;
        }

        public async Task<int> DeleteImagesAsync()
        {
            var imageSources = await GetImageSourcesForCleanAsync();
            int count = 0;

            foreach (var imageSrc in imageSources)
            {
                var absoluteImagePath = Path.Combine(uploadDirectory, imageSrc);
                var directory = new FileInfo(absoluteImagePath).Directory;
                File.Delete(absoluteImagePath);
                count++;

                if (directory.GetFiles().Length == 0)
                    directory.Delete();
            }

            return count;
        }

        private Task<List<string>> GetMessageSourcesFromASharp()
        {
            var srcList = new List<string>();
            var imageTags = dataBaseConnection.Comments
                .Where(msg => msg.Text.Contains("<img", StringComparison.OrdinalIgnoreCase)).AsEnumerable()
                .Select(async x => await htmlParser.ParseAsync(x.Text)).ToList();

            foreach (var tag in imageTags)
            {
                srcList.AddRange(tag.Result.QuerySelectorAll("img").Select(x => x.GetAttribute("src")));
            }

            return Task.FromResult(srcList);
        }

        private Task<List<string>> GetMaterialSourcesFromASharp()
        {
            var srcList = new List<string>();
            var imageTags = dataBaseConnection.Materials
                .Where(msg => msg.Text.Contains("<img", StringComparison.OrdinalIgnoreCase)).AsEnumerable()
                .Select(async x => await htmlParser.ParseAsync(x.Text)).ToList();

            foreach (var tag in imageTags)
            {
                srcList.AddRange(
                    tag.Result.QuerySelectorAll("img").Select(x => Get2LastSegments(x.GetAttribute("src"))));
            }

            return Task.FromResult(srcList);
        }

        private async Task<List<string>> GetAvatarSources()
        {
            var photos = await dataBaseConnection.Users.Select(avatar =>
                    avatar.Photo.Substring(1).Replace('/', Path.DirectorySeparatorChar))
                .ToListAsync();
            var avatars = await dataBaseConnection.Users.Select(avatar =>
                    avatar.Avatar.Substring(1).Replace('/', Path.DirectorySeparatorChar))
                .ToListAsync();

            return photos.Union(avatars).ToList();
        }

        private string Get2LastSegments(string url)
        {
            return string.Join(Path.DirectorySeparatorChar, url.Split('/').TakeLast(2));
        }
    }

    public static class DirectoryExtensions
    {
        public static void DirectoryCopy(string sourceDirName, string destDirName, bool copySubDirs)
        {
            // Get the subdirectories for the specified directory.
            DirectoryInfo dir = new DirectoryInfo(sourceDirName);

            if (!dir.Exists)
            {
                throw new DirectoryNotFoundException(
                    "Source directory does not exist or could not be found: "
                    + sourceDirName);
            }

            DirectoryInfo[] dirs = dir.GetDirectories();
            // If the destination directory doesn't exist, create it.
            if (!Directory.Exists(destDirName))
            {
                Directory.CreateDirectory(destDirName);
            }

            // Get the files in the directory and copy them to the new location.
            FileInfo[] files = dir.GetFiles();
            foreach (FileInfo file in files)
            {
                string tempPath = Path.Combine(destDirName, file.Name);

                file.CopyTo(tempPath, true);
            }

            // If copying subdirectories, copy them and their contents to new location.
            if (copySubDirs)
            {
                foreach (DirectoryInfo subDir in dirs)
                {
                    string tempPath = Path.Combine(destDirName, subDir.Name);
                    DirectoryCopy(subDir.FullName, tempPath, copySubDirs);
                }
            }
        }

        public static IEnumerable<string> GetFilesWithExcludeChildDirectory(
            string path, string searchPattern, SearchOption searchOption, Func<string, bool> expression)
        {
            List<string> paths = new List<string>();
            foreach (var childDir in Directory.GetDirectories(path))
            {
                if (expression(new DirectoryInfo(childDir).Name))
                    paths.AddRange(Directory.GetFiles(childDir, searchPattern, SearchOption.AllDirectories));
            }

            return paths;
        }
    }
}
