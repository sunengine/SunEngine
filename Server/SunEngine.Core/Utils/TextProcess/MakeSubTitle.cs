using AngleSharp.Html.Dom;

namespace SunEngine.Core.Utils.TextProcess
{
	public static class MakeSubTitle
	{
		public static string Do(IHtmlDocument doc, int subTitleLength)
		{
			if (doc.Body.TextContent.Length < subTitleLength)
				return doc.Body.TextContent;
			else
				return doc.Body.TextContent.Substring(0, subTitleLength) + "...";
		}
	}
}