using LinqToDB.Identity;

namespace SunEngine.Models.Authorization
{
    public class UserToGroup : IdentityUserRole<int>
    {
        public User User { get; set; }

        public UserGroup UserGroup { get; set; }
    }
}