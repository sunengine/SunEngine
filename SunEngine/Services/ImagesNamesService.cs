using System;
using System.IO;
using System.Text;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Options;
using SunEngine.Configuration.Options;
using SunEngine.Security.Cryptography;

namespace SunEngine.Services
{
    public interface IImagesNamesService
    {
        FileAndDir GetNewImageNameAndDir(string ext);
    }
    
    public class FileAndDir
    {
        public string File { get; } 
        public string Dir { get; }
        public string Path { get; }

        public FileAndDir(string file, string dir)
        {
            this.File = file;
            this.Dir = dir;
            this.Path = System.IO.Path.Combine(dir,file);
        }

        public override string ToString()
        {
            return Path;
        }
    }
    
    public class ImagesNamesService : IImagesNamesService
    {
        private readonly string basePath;
        
        public ImagesNamesService(IHostingEnvironment env, IOptions<ImagesOptions> imagesOptions)
        {
            basePath = Path.Combine(env.WebRootPath, imagesOptions.Value.UploadDir);
        }
        
        public FileAndDir GetNewImageNameAndDir(string ext)
        {
            while (true)
            {
                //var guid = Guid.NewGuid();
                //var bites = guid.ToByteArray();
                //return new FileAndDir(guid.ToString("N") + ext, bites[0].ToString());
                
                
                var cid = CryptoRandomizer.GetRandomString(12);
                byte[] bites = Encoding.UTF8.GetBytes(cid);   
                
                FileAndDir rez = new FileAndDir(cid + ext, bites[0].ToString());
                if (!CheckFileOnDisk(rez.Path))
                    return rez;
            }           
        }

        private bool CheckFileOnDisk(string path)
        {
            var fullPath = Path.Combine(basePath, path);
            return File.Exists(fullPath);
        }
    }
    
}