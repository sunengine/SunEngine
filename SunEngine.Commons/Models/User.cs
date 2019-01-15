using LinqToDB.Identity;

namespace SunEngine.Commons.Models
{
    public enum Sex { Unset = 0, Male = 1, Female = 2}
    
    public class User : IdentityUser<int>
    {
        public static string DefaultAvatar => "_/default-avatar.svg";

        /// <summary>
        /// токен для ссылки на пользователя site.com/user/DmitrijP 
        /// </summary>
        public string Link { get; set; }
        
        public void SetDefaultLink()
        {
            Link = Id.ToString();
        }
        
        public string Information { get; set; }

        public string Photo { get; set; }
        
        public string Avatar { get; set; }
    }
}
