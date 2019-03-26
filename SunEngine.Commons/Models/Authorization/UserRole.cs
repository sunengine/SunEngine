using LinqToDB.Identity;

namespace SunEngine.Commons.Models.Authorization
{
    public class UserRole : IdentityUserRole<int>
    {
        public User User { get; set; }

        public Role Role { get; set; }
    }
}