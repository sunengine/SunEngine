using System.Linq;
using AngleSharp.Dom;
using AngleSharp.Dom.Html;
using AngleSharp.Parser.Html;
using Ganss.XSS;

namespace SunEngine.Commons.TextProcess
{
    public static class SanitizerExtension
    {
        public static string MySanitize(this HtmlSanitizer htmlSanitizer, string html)
        {
            string rez;
            rez = htmlSanitizer.Sanitize(html);
            //rez = ReplaceVoidParagraphs(rez);
            return rez;
        }

        #region ReplaceVoidParagraphs

        private static string ReplaceVoidParagraphs(string html)
        {
            HtmlParser parser = new HtmlParser();
            IHtmlDocument doc = parser.Parse(html);
            var ps = doc.QuerySelectorAll("p,h1,h2,h3,h4,h5,h6");
            foreach (var p in ps)
            {
                DetectVoidParagraph(doc, p);
            }
            return doc.Body.InnerHtml;
        }

        private static readonly string[] voidptags = { "p", "h1", "h2", "h3", "h4", "h5", "h6", "span", "b", "i", "strong", "em" };

        private static void DetectVoidParagraph(IHtmlDocument doc, IElement ell)
        {
            if (!voidptags.Contains(ell.TagName.ToLower()))
                return;
            if (ell.Children.Length == 0 && string.IsNullOrWhiteSpace(ell.TextContent))
            {
                var br = doc.CreateElement("br");
                br.ClassName = "breakline";
                ell.AppendChild(br);
                return;
            }
            if (ell.Children.Length == 1)
            {
                DetectVoidParagraph(doc, ell.Children[0]);
                return;
            }
            return;
        }
        #endregion
    }
}
