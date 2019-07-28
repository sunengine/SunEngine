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
                x => !x.Contains("_")).Select(Get2LastSegments).ToList();

            var allSources = await Task.Factory.ContinueWhenAll(
                new[]
                {
                    GetMaterialSourcesFromASharpAsync(),
                    GetCommentsSourcesFromASharpAsync(),
                    GetAvatarSourcesAsync()
                }, tasks =>
                    new List<string>(tasks.SelectMany(t => t.Result)));

            return new List<string>(imagesInDirectory.Where(imagePath => !allSources.Contains(imagePath)));
        }

        public async ValueTask<int> DeleteImagesAsync()
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

        private Task<IEnumerable<string>> GetCommentsSourcesFromASharpAsync()
        {
            var imageTags = dataBaseConnection.Comments
                .Where(msg => msg.Text.Contains("<img", StringComparison.OrdinalIgnoreCase)).AsEnumerable()
                .Select(x => htmlParser.ParseAsync(x.Text)).ToArray();

            if (imageTags.Length > 0)
                return Task.Factory.ContinueWhenAll(imageTags, tasks =>
                    tasks.SelectMany(t =>
                        t.Result.QuerySelectorAll("img").Select(x => Get2LastSegments(x.GetAttribute("src")))));

            return Task.FromResult(Array.Empty<string>().AsEnumerable());
        }

        private Task<IEnumerable<string>> GetMaterialSourcesFromASharpAsync()
        {
            var imageTags = dataBaseConnection.Materials
                .Where(msg => msg.Text.Contains("<img", StringComparison.OrdinalIgnoreCase)).AsEnumerable()
                .Select(x => htmlParser.ParseAsync(x.Text)).ToArray();

            if (imageTags.Length > 0)
                return Task.Factory.ContinueWhenAll(imageTags, tasks =>
                    tasks.SelectMany(t =>
                        t.Result.QuerySelectorAll("img").Select(x => Get2LastSegments(x.GetAttribute("src")))));

            return Task.FromResult(Array.Empty<string>().AsEnumerable());
        }

        private async Task<IEnumerable<string>> GetAvatarSourcesAsync()
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
        public static IEnumerable<string> GetFilesWithExcludeChildDirectory(
            string initialPathToDirectory, string fileSearchPattern, Func<string, bool> expression)
        {
            foreach (var pathToChildDirectory in Directory.GetDirectories(initialPathToDirectory).Where(expression))
            foreach (var pathToFile in Directory.GetFiles(pathToChildDirectory, fileSearchPattern,
                SearchOption.AllDirectories))
                yield return pathToFile;
        }
    }
}
