using LinqToDB.Identity;

namespace SunEngine.Commons.Models.Authorization
{
    /// <summary>
    /// Many to many relationship between User and Role
    /// </summary>
    public class UserRole : IdentityUserRole<int>
    {
        public User User { get; set; }

        public Role Role { get; set; }
    }
}