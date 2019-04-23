using Ganss.XSS;

namespace SunEngine.Commons.Utils.TextProcess
{

    public class Sanitizer
    {
        private readonly HtmlSanitizer htmlSanitizer;
        private readonly string siteUrl = "";

        private readonly string[] allowedTags = { "a", "b" ,"strong", "i", "em", "blockquote", "ol","li","ul", "ol", "p",
            "div", "br","video","audio","source","span", "img", "code", "pre", "font" };

        private readonly string[] allowedAttributes = { "style", "src", "controls", "autoplay", "loop", "alt",
            "width", "height", "target", "frameborder", "allowfullscreen", "download", "controlsList", "size"};

        private readonly string[] allowedClasses = { "text-img" };

        private readonly string[] allowedCssProperties = { "float", "margin", "indent", "padding", "color",
            "text-decoration", "font-size", "width", "height", "max-width" };

        public Sanitizer()
        {
            htmlSanitizer = new HtmlSanitizer();

            htmlSanitizer.AllowedTags.Clear();

            foreach(string tag in allowedTags)
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
            bool x = SanitizerBlocksAttributes.AllowOnlyClassList(attributeName, e,allowedClasses) 
                || SanitizerBlocksAttributes.MakeExternalLinksOpenedNewTab(attributeName, e,siteUrl);
        }

        private void ForumSanitizer_RemovingTag(object sender, RemovingTagEventArgs e)
        {
            string tagName = e.Tag.TagName.ToLower();
            bool x = SanitizerBlocksTags.CheckIframeAllowedDomens(tagName,e) 
                || SanitizerBlocksTags.AddImgClasses(tagName,e);
            
        }

        public string Sanitize(string text)
        {
            if(string.IsNullOrWhiteSpace(text))
                return null;
            return htmlSanitizer.Sanitize(text);
        }
    }
}
