using System.Text.RegularExpressions;

namespace SunEngine.Core.Utils.TextProcess
{
	public static class SimpleHtmlToText
	{
		static readonly Regex regexTagsAndBreaks = new Regex("<.+?>|<.+?/>|\n|\t");
		static readonly Regex regexTags = new Regex("<.+?>|<.+?/>");


		public static string ClearTagsAndBreaks(string htmlPart)
		{
			if (string.IsNullOrWhiteSpace(htmlPart))
				return null;
			return regexTagsAndBreaks.Replace(htmlPart, " ").Trim();
		}

		public static string ClearTags(string htmlPart)
		{
			if (string.IsNullOrWhiteSpace(htmlPart))
				return null;
			return regexTags.Replace(htmlPart, " ").Trim();
		}
	}
}