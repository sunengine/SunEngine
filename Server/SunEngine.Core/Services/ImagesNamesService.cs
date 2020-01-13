using System.Text;
using SunEngine.Core.DataBase;
using SunEngine.Core.Utils;

namespace SunEngine.Core.Services
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
			this.Path = System.IO.Path.Combine(dir, file);
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
			var cid = CryptoRandomizer.GetRandomString(
				DbColumnSizes.FileNameWithDirSize -
				8); // Why -8. 4 needed for directory start "123/", and 4 needed for extension ".jpg"
			byte[] bites = Encoding.UTF8.GetBytes(cid);

			return new FileAndDir(cid + ext, bites[0].ToString());
		}
	}
}