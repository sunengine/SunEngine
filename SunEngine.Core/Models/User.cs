using System;
using System.Collections.Generic;
using LinqToDB.Identity;
using LinqToDB.Mapping;

namespace SunEngine.Core.Models
{
    public enum Sex
    {
        Unset = 0,
        Male = 1,
        Female = 2
    }

    public class User : IdentityUser<int>
    {
        /// <summary>
        /// Path to default avatar in "UploadImages" folder
        /// </summary>
        public static string DefaultAvatar => "default-avatar.svg";

        /// <summary>
        /// Token for link like 'MyUserNameLink': https://site.com/user/MyUserNameLink
        /// Sets to User.Id when default 
        /// </summary>
        public string Link { get; set; }

        /// <summary>
        /// Anything user wants to say about himself
        /// </summary>
        public string Information { get; set; }

        /// <summary>
        /// Photo to display on profile
        /// </summary>
        public string Photo { get; set; }

        /// <summary>
        /// Avatar to display on Client
        /// </summary>
        public string Avatar { get; set; }

        public DateTime RegisteredDate { get; set; }

        public int ProfileVisitsCount { get; set; }

        /// <summary>
        /// Who is banned by this user
        /// </summary>
        [Association(ThisKey = "Id", OtherKey = "UserId")]
        public ICollection<UserBanedUnit> BanList { get; set; }
    }

    /// <summary>
    /// Many to many relationship on users UserId (field) baned UserBannedId (filed)
    /// </summary>
    public class UserBanedUnit
    {
        public int UserId { get; set; }

        /// <summary>
        /// This user ban UserBaned (filed)
        /// </summary>
        public User User { get; set; }

        public int UserBanedId { get; set; }

        /// <summary>
        /// This user baned by User (field)
        /// </summary>
        public User UserBaned { get; set; }
    }
}
