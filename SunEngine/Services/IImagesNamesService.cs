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
}