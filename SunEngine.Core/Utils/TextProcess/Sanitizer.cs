using System;
using System.Linq;
using AngleSharp.Dom.Html;
using AngleSharp.Extensions;
using Ganss.XSS;
using SunEngine.Core.Configuration.Options;

namespace SunEngine.Core.Utils.TextProcess
{
    // TODO Transfer to Services
    public class Sanitizer
    {
        private readonly HtmlSanitizer htmlSanitizer;
        private readonly SanitizerOptions options;
        private readonly string siteUrl = "";

        public Sanitizer(SanitizerOptions options)
        {
            this.options = options;
            this.htmlSanitizer = new HtmlSanitizer();
            this.ConfigureHtmlSanitizer();
        }

        public string Sanitize(string text)
        {
            return string.IsNullOrWhiteSpace(text) ? null : htmlSanitizer.Sanitize(text);
        }

        public string Sanitize(IHtmlDocument doc)
        {
            return doc == null ? null : htmlSanitizer.SanitizeDoc(doc);
        }

        private void ConfigureHtmlSanitizer()
        {
            htmlSanitizer.AllowedTags.Clear();

            foreach (string tag in options.AllowedTags)
            {
                htmlSanitizer.AllowedTags.Add(tag);
            }

            htmlSanitizer.AllowedAttributes.Clear();

            foreach (string attribute in options.AllowedAttributes)
            {
                htmlSanitizer.AllowedAttributes.Add(attribute);
            }

            htmlSanitizer.AllowedCssProperties.Clear();

            foreach (string cssp in options.AllowedCssProperties)
            {
                htmlSanitizer.AllowedCssProperties.Add(cssp);
            }

            htmlSanitizer.AllowedSchemes.Add("mailto");

            htmlSanitizer.AllowedAtRules.Clear();

            htmlSanitizer.AllowedTags.Remove("img");
            htmlSanitizer.RemovingTag += OnRemovingTag;
            htmlSanitizer.RemovingAttribute += OnRemovingAttribute;
        }
        
        private bool CheckIframeAllowedDomens(string tagName, RemovingTagEventArgs e)
        {
            if (tagName == "iframe") // вроверяем куда ведёт iframe src, блокируем
                // всё, кроме разрешённых сайтов
            {
                var src = e.Tag.GetAttribute("src").TrimStart().ToLower();
                foreach (var allowedDomen in options.AllowedVideoDomains)
                {
                    if (src.StartsWith(allowedDomen))
                    {
                        e.Cancel = true;
                        return true;
                    }
                }

                e.Cancel = false;
                return true;
            }

            return false;
        }

        private bool AddImgClasses(string tagName, RemovingTagEventArgs e)
        {
            if (tagName == "img") // в любую картинку добавляем img-responsive
            {
                if (!e.Tag.GetAttribute("src").Contains("emoticons")) // Кроме смайликов
                {
                    e.Tag.ClassList.Add("text-img");
                }

                e.Cancel = true;
            }

            return false;
        }

        private static bool AllowOnlyClassList(string attributeName, RemovingAttributeEventArgs e,
            string[] allowedClasses)
        {
            if (attributeName == "class")
            {
                e.Tag.ClassList.Remove(e.Tag.ClassList.Except(allowedClasses, StringComparer.OrdinalIgnoreCase)
                    .ToArray());
                e.Cancel = e.Tag.ClassList.Any();
                return true;
            }

            return false;
        }

        private static bool MakeExternalLinksOpenedNewTab(string attributeName, RemovingAttributeEventArgs e,
            string siteUrl)
        {
            if (attributeName == "href")
            {
                var href = e.Attribute.Value;
                if (!href.StartsWith(siteUrl) &&
                    (href.StartsWith("http") || href.StartsWith("https")
                                             || href.StartsWith("ftp")))
                {
                    if (!e.Tag.Attributes.Any(x => x.Name.ToLower() == "target"))
                    {
                        e.Tag.SetAttribute("target", "_blank");
                    }
                }

                e.Cancel = true;
                return true;
            }

            return false;
        }

        #region EventHandlers

        private void OnRemovingAttribute(object s, RemovingAttributeEventArgs e)
        {
            var attributeName = e.Attribute.Name.ToLower();
            var _ = AllowOnlyClassList(attributeName, e, options.AllowedClasses)
                    || MakeExternalLinksOpenedNewTab(attributeName, e, siteUrl);
        }

        private void OnRemovingTag(object sender, RemovingTagEventArgs e)
        {
            var tagName = e.Tag.TagName.ToLower();
            var _ = CheckIframeAllowedDomens(tagName, e)
                    || AddImgClasses(tagName, e);
        }

        #endregion
    }

    public static class SanitizerExtensions
    {
        public static string SanitizeDoc(this HtmlSanitizer htmlSanitizer, IHtmlDocument doc)
        {
            return doc.Body.ChildNodes.ToHtml(htmlSanitizer.OutputFormatter);
        }
    }
}