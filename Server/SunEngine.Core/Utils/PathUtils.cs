using System.Text.RegularExpressions;

namespace SunEngine.Core.Utils
{
	public static class PathUtils
	{
		static readonly Regex ClearTokenRegex = new Regex("^[^a-zA-Z0-9_-]+$");

		public static string ClearPathToken(string token)
		{
			return ClearTokenRegex.Replace(token, "");
		}
	}
}