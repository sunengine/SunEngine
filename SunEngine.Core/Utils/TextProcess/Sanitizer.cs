using AngleSharp.Dom.Html;
using AngleSharp.Extensions;
using Ganss.XSS;

namespace SunEngine.Core.Utils.TextProcess
{
    public class Sanitizer
    {
        private readonly HtmlSanitizer htmlSanitizer;
        private readonly string siteUrl = "";

        private readonly string[] allowedTags =
        {
            "a", "b", "strong", "i", "em", "blockquote", "ol", "li", "ul", "ol", "p",
            "div", "br", "video", "audio", "source", "span", "img", "code", "pre", "font", "h3", "h4", "h5", "h6"
        };

        private readonly string[] allowedAttributes =
        {
            "style", "src", "controls", "autoplay", "loop", "alt",
            "width", "height", "target", "frameborder", "allowfullscreen", "download", "controlsList", "size"
        };

        private readonly string[] allowedClasses = {"text-img"};

        private readonly string[] allowedCssProperties =
        {
            "float", "margin", "indent", "padding", "color", "text-align",
            "text-decoration", "font-size", "width", "height", "max-width"
        };

        public Sanitizer()
        {
            htmlSanitizer = new HtmlSanitizer();

            htmlSanitizer.AllowedTags.Clear();

            foreach (string tag in allowedTags)
            {
                htmlSanitizer.AllowedTags.Add(tag);
            }

            htmlSanitizer.AllowedAttributes.Clear();

            foreach (string attribute in allowedAttributes)
            {
                htmlSanitizer.AllowedAttributes.Add(attribute);
            }

            htmlSanitizer.AllowedCssProperties.Clear();

            foreach (string cssp in allowedCssProperties)
            {
                htmlSanitizer.AllowedCssProperties.Add(cssp);
            }

            htmlSanitizer.AllowedSchemes.Add("mailto");

            htmlSanitizer.AllowedAtRules.Clear();

            htmlSanitizer.AllowedTags.Remove("img");
            htmlSanitizer.RemovingTag += ForumSanitizer_RemovingTag;
            htmlSanitizer.RemovingAttribute += ForumSanitizer_RemovingAttribute_Forum;
        }


        private void ForumSanitizer_RemovingAttribute_Forum(object s, RemovingAttributeEventArgs e)
        {
            var attributeName = e.Attribute.Name.ToLower();
            var _ = SanitizerBlocksAttributes.AllowOnlyClassList(attributeName, e, allowedClasses)
                    || SanitizerBlocksAttributes.MakeExternalLinksOpenedNewTab(attributeName, e, siteUrl);
        }

        private void ForumSanitizer_RemovingTag(object sender, RemovingTagEventArgs e)
        {
            string tagName = e.Tag.TagName.ToLower();
            var _ = SanitizerBlocksTags.CheckIframeAllowedDomens(tagName, e)
                    || SanitizerBlocksTags.AddImgClasses(tagName, e);
        }

        public string Sanitize(string text)
        {
            return string.IsNullOrWhiteSpace(text) ? 
                null : 
                htmlSanitizer.Sanitize(text);
        }

        public string Sanitize(IHtmlDocument doc)
        {
            return doc == null ?
                null : 
                htmlSanitizer.SanitizeDoc(doc);
        }
    }

    public static class SanitizerExtensions
    {
        public static string SanitizeDoc(this HtmlSanitizer htmlSanitizer, IHtmlDocument doc)
        {
            return doc.Body.ChildNodes.ToHtml(htmlSanitizer.OutputFormatter);
        }
    }
}
