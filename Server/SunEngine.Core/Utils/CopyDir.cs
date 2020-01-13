using System.IO;

namespace SunEngine.Core.Utils
{
	public static class CopyDir
	{
		public static void Copy(string fromPath, string toPath)
		{
			foreach (string dirPath in Directory.GetDirectories(fromPath, "*",
				SearchOption.AllDirectories))
				Directory.CreateDirectory(dirPath.Replace(fromPath, toPath));

			foreach (string newPath in Directory.GetFiles(fromPath, "*.*",
				SearchOption.AllDirectories))
				System.IO.File.Copy(newPath, newPath.Replace(fromPath, toPath), true);
		}
	}
}