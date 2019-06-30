using AngleSharp.Dom.Html;

namespace SunEngine.Core.Utils.TextProcess
{
    public static class MakeSubTitle
    {
        public static string Do(IHtmlDocument doc, int subTitleLength)
        {
            if(doc.TextContent.Length < subTitleLength)
                return doc.TextContent;
            else
                return doc.TextContent.Substring(0, subTitleLength) + "...";
        }
    }
}
