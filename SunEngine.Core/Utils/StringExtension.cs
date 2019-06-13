namespace SunEngine.Core.Utils
{
    public static class StringExtension
    {
        public static string SetNullIfEmptyTrim(this string str)
        {
            str = str.Trim();
            return str == "" ? null : str;
        }
    }
}
