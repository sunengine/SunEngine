using Ganss.XSS;

namespace SunEngine.Core.Utils.TextProcess
{
    public static class SanitizerBlocksTags
    {

        // ---------- TAGS ------------

        private static readonly string[] allowedVideoDomens = { "https://www.youtube.com/", "http://www.youtube.com/",
            "https://youtube.com/", "http://youtube.com/", "https://youtu.be/", "http://youtu.be/",
            "//youtube.com/", "//youtu.be/", "//www.youtube.com/", "//www.youtu.be/",
            "https://vk.com/", "http://vk.com/", "//vk.com/",
            "https://player.vimeo.com", "http://player.vimeo.com", "//player.vimeo.com"
        };

        public static bool CheckIframeAllowedDomens(string tagName, RemovingTagEventArgs e)
        {
            if (tagName == "iframe") // вроверяем куда ведёт iframe src, блокируем
                                     // всё, кроме разрешённых сайтов
            {
                string src = e.Tag.GetAttribute("src").TrimStart().ToLower();
                foreach (var allowedDomen in allowedVideoDomens)
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

        public static bool AddImgClasses(string tagName, RemovingTagEventArgs e)
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

       
    }
}
