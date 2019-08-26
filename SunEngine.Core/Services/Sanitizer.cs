using System;
using System.Collections.Generic;
using System.Linq;
using AngleSharp.Dom.Html;
using AngleSharp.Extensions;
using Ganss.XSS;
using SunEngine.Core.Configuration.Options;

namespace SunEngine.Core.Services
{
    public class Sanitizer
    {
        private readonly HtmlSanitizer htmlSanitizer;
        private readonly SanitizerOptions options;

        public Sanitizer(SanitizerOptions options)
        {
            this.options = options;
            htmlSanitizer = new HtmlSanitizer();
            ConfigureHtmlSanitizer();
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
                htmlSanitizer.AllowedTags.Add(tag);

            htmlSanitizer.AllowedAttributes.Clear();

            foreach (string attribute in options.AllowedAttributes)
                htmlSanitizer.AllowedAttributes.Add(attribute);

            htmlSanitizer.AllowedCssProperties.Clear();

            foreach (string cssp in options.AllowedCssProperties)
                htmlSanitizer.AllowedCssProperties.Add(cssp);

            htmlSanitizer.AllowedSchemes.Add("mailto");

            htmlSanitizer.AllowedAtRules.Clear();

            htmlSanitizer.RemovingTag += OnRemovingTag;
            htmlSanitizer.RemovingAttribute += OnRemovingAttribute;
        }

        private bool CheckAllowedDomains(string attrName, IEnumerable<string> allowedDomains, RemovingTagEventArgs e)
        {
            var src = e.Tag.GetAttribute(attrName).TrimStart().ToLower();
            foreach (var allowedDomain in allowedDomains)
                if (src.StartsWith(allowedDomain))
                    return e.Cancel = true;

            e.Cancel = false;
            return false;
        }

        private bool AllowOnlyClassList(
            string attributeName, RemovingAttributeEventArgs e,
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

        private void OnRemovingAttribute(object s, RemovingAttributeEventArgs e)
        {
            var attributeName = e.Attribute.Name.ToLower();
            AllowOnlyClassList(attributeName, e, options.AllowedClasses);
        }

        private void OnRemovingTag(object sender, RemovingTagEventArgs e)
        {
            var checkingTags = new[]
            {
                new { Tag = "iframe", Attribute = "src", AllowedDomainsList = options.AllowedVideoDomains },
                new { Tag = "img", Attribute = "src", AllowedDomainsList = options.AllowedImageDomains }
            };
            
            var tagName = e.Tag.TagName.ToLower();
            var tag = checkingTags.FirstOrDefault(x => x.Tag == tagName);
            
            if (tag != null)
                CheckAllowedDomains(tag.Attribute, tag.AllowedDomainsList, e);
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
