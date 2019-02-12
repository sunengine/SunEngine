using System;

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

        public FileAndDir GetNewImageNameAndDir(string ext)
        {
            var guid = Guid.NewGuid();
            var bites = guid.ToByteArray();
            return new FileAndDir(guid + ext, bites[0].ToString());
        }

    }
    
}