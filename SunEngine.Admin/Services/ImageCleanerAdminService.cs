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
            var imagesInDirectory = DirectoryExtensions.GetFilesWithExcludeChildDirectory(uploadDirectory, "*.*",
                "_*").Select(Get2LastSegments).ToList();
            
            var materialSources = await GetMaterialSourcesFromASharp();
            var msgSources = await GetMessageSourcesFromASharp();
            var avatarSources = await GetAvatarSources();

            var allSources = await Task.Factory.ContinueWhenAll(new[] { GetMaterialSourcesFromASharp(), GetMessageSourcesFromASharp(), GetAvatarSources() }, tasks =>
                new List<string>(tasks.SelectMany(t => t.Result)));

            return new List<string>(imagesInDirectory.Where(imagePath => !allSources.Contains(imagePath)));
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

        private Task<IEnumerable<string>> GetMessageSourcesFromASharp()
        {
            var imageTags = dataBaseConnection.Comments
                .Where(msg => msg.Text.Contains("<img", StringComparison.OrdinalIgnoreCase)).AsEnumerable()
                .Select(async x => await htmlParser.ParseAsync(x.Text)).ToList();

            return Task.Factory.ContinueWhenAll(imageTags.ToArray(), tasks =>
                tasks.SelectMany(t => t.Result.QuerySelectorAll("img").Select(x => x.GetAttribute("src"))));
        }

        private Task<IEnumerable<string>> GetMaterialSourcesFromASharp()
        {
            var imageTags = dataBaseConnection.Materials
                .Where(msg => msg.Text.Contains("<img", StringComparison.OrdinalIgnoreCase)).AsEnumerable()
                .Select(async x => await htmlParser.ParseAsync(x.Text)).ToList();

            return Task.Factory.ContinueWhenAll(imageTags.ToArray(), tasks =>
                tasks.SelectMany(t => t.Result.QuerySelectorAll("img").Select(x => Get2LastSegments(x.GetAttribute("src")))));
        }

        private async Task<IEnumerable<string>> GetAvatarSources()
        {
            var photos = await dataBaseConnection.Users.Select(avatar =>
                    avatar.Photo.Replace('/', Path.DirectorySeparatorChar))
                .ToListAsync();
            var avatars = await dataBaseConnection.Users.Select(avatar =>
                    avatar.Avatar.Replace('/', Path.DirectorySeparatorChar))
                .ToListAsync();

            return photos.Union(avatars).ToList();
        }

        private string Get2LastSegments(string url)
        {
            return string.Join(Path.DirectorySeparatorChar, url.Split(Path.DirectorySeparatorChar).TakeLast(2));
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
            string initialPathToDirectory, string fileSearchPattern, string directorySearchPattern)
        {
            foreach (var pathToChildDirectory in Directory.GetDirectories(initialPathToDirectory, directorySearchPattern))
                foreach (var pathToFile in Directory.GetFiles(pathToChildDirectory, fileSearchPattern, SearchOption.AllDirectories))
                    yield return pathToFile;
        }
    }
}
