using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using LinqToDB;
using SunEngine.Core.DataBase;
using AngleSharp.Parser.Html;
using Microsoft.Extensions.Options;
using SunEngine.Core.Configuration.Options;

namespace SunEngine.Admin.Services
{
    public class ImageCleanerAdminService
    {
        private readonly DataBaseConnection _dataBaseConnection;
        private readonly ImagesOptions _imagesOptions;
        private readonly HtmlParser _htmlParser;        

        public ImageCleanerAdminService(DataBaseConnection dataBaseConnection, IOptions<ImagesOptions> imagesOptions)
        {
            _dataBaseConnection = dataBaseConnection;
            _imagesOptions = imagesOptions.Value;
            _htmlParser = new HtmlParser();            
        }

        #region Public

        public async Task<int> DeleteImagesAsync()
        {
            var imageSources = await GetImageSourcesForCleanAsync();
            int count = 0;

            foreach (var imageSrc in imageSources)
            {
                try
                {                    
                    var absoluteImagePath = Path.Combine(_imagesOptions.UploadDir, imageSrc);
                    var directory = new FileInfo(absoluteImagePath).Directory;
                    File.Delete(absoluteImagePath);
                    count++;

                    if (directory.GetFiles().Length == 0)
                        directory.Delete();
                }
                catch (Exception)
                {
                    throw;
                }
            }
            return count;
        }        
        
        public async Task<List<string>> GetImageSourcesForCleanAsync()
        {
            List<string> sources = new List<string>();
            
            var imagesInDirectory = DirectoryExtensions.GetFilesWithExcludeChildDirectory(_imagesOptions.UploadDir, "*.*",
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

        #endregion

        #region Private

        private Task<List<string>> GetMessageSourcesFromASharp()
        {
            var srcList = new List<string>();
            var imageTags = _dataBaseConnection.Comments
                .Where(msg => msg.Text.Contains("<img", StringComparison.OrdinalIgnoreCase)).AsEnumerable()
                .Select(  async x => await _htmlParser.ParseAsync(x.Text)).ToList();

            foreach (var tag in imageTags)
            {
                srcList.AddRange(tag.Result.QuerySelectorAll("img").Select(x => x.GetAttribute("src")));
            }

            return Task.FromResult(srcList);
        }

        private Task<List<string>> GetMaterialSourcesFromASharp()
        {
            var srcList = new List<string>();
            var imageTags = _dataBaseConnection.Materials
                .Where(msg => msg.Text.Contains("<img", StringComparison.OrdinalIgnoreCase)).AsEnumerable()
                .Select(async x => await _htmlParser.ParseAsync(x.Text)).ToList();

            foreach (var tag in imageTags)
            {
                srcList.AddRange(tag.Result.QuerySelectorAll("img").Select(x => x.GetAttribute("src")));
            }

            return Task.FromResult(srcList);
        }

        private async Task<List<string>> GetAvatarSources()
        {
            var photos = await _dataBaseConnection.Users.Select(avatar => avatar.Photo.Substring(1).Replace('/','\\')).ToListAsync();
            var avatars = await _dataBaseConnection.Users.Select(avatar => avatar.Avatar.Substring(1).Replace('/', '\\')).ToListAsync();

            return photos.Union(avatars).ToList();
        }

        #endregion
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
                string temppath = Path.Combine(destDirName, file.Name);
                
                file.CopyTo(temppath, true);
            }

            // If copying subdirectories, copy them and their contents to new location.
            if (copySubDirs)
            {
                foreach (DirectoryInfo subdir in dirs)
                {
                    string temppath = Path.Combine(destDirName, subdir.Name);
                    DirectoryCopy(subdir.FullName, temppath, copySubDirs);
                }
            }
        }

        public static IEnumerable<string> GetFilesWithExcludeChildDirectory(string path, string searchPattern, SearchOption searchOption, Func<string,bool> expression)
        {
            List<string> paths = new List<string>();
            foreach (var childDir in Directory.GetDirectories(path))
            {
                if(expression(new DirectoryInfo(childDir).Name))
                    paths.AddRange(Directory.GetFiles(childDir, searchPattern, SearchOption.AllDirectories));
            }
            return paths;
        }
    }
}
