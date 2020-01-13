using AngleSharp.Dom.Html;

namespace SunEngine.Core.Utils.TextProcess
{
	public static class MakeSubTitle
	{
		public static string Do(IHtmlDocument doc, int subTitleLength)
		{
			if (doc.Body.InnerText.Length < subTitleLength)
				return doc.Body.InnerText;
			else
				return doc.Body.InnerText.Substring(0, subTitleLength) + "...";
		}
	}
}