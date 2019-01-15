using System;
using System.IO;

namespace SunEngine.Services
{
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