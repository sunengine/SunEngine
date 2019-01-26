using System.Collections.Generic;
using LinqToDB.Identity;
using LinqToDB.Mapping;

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
        
        [Association(ThisKey = "Id", OtherKey = "UserId")]
        public ICollection<UserBanedUnit> BanList { get; set; }
    }

    public class UserBanedUnit
    {
        public int UserId { get; set; }
        public User User { get; set; }
        
        public int UserBanedId { get; set; }
        public User UserBaned { get; set; }
    }
}
