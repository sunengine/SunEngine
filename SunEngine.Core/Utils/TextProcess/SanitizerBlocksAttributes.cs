using System;
using System.Linq;
using Ganss.XSS;

namespace SunEngine.Core.Utils.TextProcess
{
    public static class SanitizerBlocksAttributes
    {

        public static bool AllowOnlyClassList(string attributeName, RemovingAttributeEventArgs e, string[] allowedClasses)
        {
            if (attributeName == "class")
            {
                e.Tag.ClassList.Remove(e.Tag.ClassList.Except(allowedClasses, StringComparer.OrdinalIgnoreCase).ToArray());
                e.Cancel = e.Tag.ClassList.Any();
                return true;
            }
            else
            {
                return false;
            }
        }

        public static bool MakeExternalLinksOpenedNewTab(string attributeName, RemovingAttributeEventArgs e, string siteUrl)
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
    }
}
