using System.Text.RegularExpressions;

namespace SunEngine.Utils.TextProcess
{
    public static class SimpleHtmlToText
    {
        static readonly Regex regex = new Regex("<.+?>|<.+?/>|\n|\t|<.*?$");
        
        public static string Convert(string htmlPart)
        {
            return regex.Replace(htmlPart, " ").Trim();
        }
    }
}