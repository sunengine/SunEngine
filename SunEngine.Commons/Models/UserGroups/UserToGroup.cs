using LinqToDB.Identity;

namespace SunEngine.Commons.Models.UserGroups
{
    public class UserToGroup : IdentityUserRole<int>
    {
        public User User { get; set; }

        public UserGroupDB UserGroup { get; set; }
    }
}