using System.Text.Json;

namespace SunEngine.Core.Utils
{
	public static class StringExtension
	{
		public static string SetNullIfEmptyTrim(this string str)
		{
			str = str.Trim();
			return str == "" ? null : str;
		}

		public static string SetNullIfEmpty(this string str)
		{
			return str == "" ? null : str;
		}

		public static string MakeJsonText(this string json)
		{
			if (json == null)
				return null;

			json = json.Trim();

			if (json == "")
				return null;

			if (ValidateJson(json))
				return json;

			return null;
		}

		public static string NullJson => "{}";

		public static string MakeJsonTextNotNull(this string json)
		{
			if (json == null)
				return NullJson;

			json = json.Trim();

			if (json == "")
				return NullJson;

			if (ValidateJson(json))
				return json;

			return NullJson;
		}

		public static bool ValidateJson(this string json)
		{
			try
			{
				JsonDocument.Parse(json);
				return true;
			}
			catch
			{
				return false;
			}
		}

		public static string ToSnakeCase(this string str)
		{
			return str;
			//return System.Text.RegularExpressions.Regex.Replace(str, "(?<=.)([A-Z])", "_$0",
			//    System.Text.RegularExpressions.RegexOptions.Compiled);
		}
	}
}