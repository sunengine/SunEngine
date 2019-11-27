using System.Linq;
using System.Threading.Tasks;
using LinqToDB;
using SunEngine.Core.DataBase;
using SunEngine.Core.Presenters;
using SunEngine.Core.Security;
using SunEngine.Core.Services;
using SunEngine.Core.Utils;

namespace SunEngine.Admin.Presenters
{
    public interface IUserRolesAdminPresenter
    {
        Task<RoleView[]> GetAllRolesAsync();
        Task<UserInfoView[]> GetRoleUsers(string groupName, string userNamePart);
        Task<RoleView[]> GetUserRolesAsync(int userId);
    }

    public class UserRolesAdminPresenter : DbService, IUserRolesAdminPresenter
    {
        private const int MaxUsersTake = 40;
        
        
        public UserRolesAdminPresenter(DataBaseConnection db) : base(db)
        {
        }

        public Task<RoleView[]> GetAllRolesAsync()
        {
            return db.Roles.Where(x=>x.NormalizedName !=  RoleNames.UnregisteredNormalized).Select(x => new RoleView
            {
                Name = x.Name,
                Title = x.Title,
                UsersCount = x.Users.Count,
                IsSuper = x.IsSuper
            }).ToArrayAsync();
        }

        public Task<UserInfoView[]> GetRoleUsers(string roleName, string userNamePart)
        {
            var normalizedGroupName = Normalizer.Normalize(roleName);
            var query = db.UserRoles.Where(x => x.Role.NormalizedName == normalizedGroupName);

            if (userNamePart != null)
            {
                userNamePart = userNamePart.Trim().ToLower();
                if (!string.IsNullOrEmpty(userNamePart))
                {
                    query = query.Where(x => x.User.UserName.ToLower().Contains(userNamePart));
                }
            }

            return query
                .Take(MaxUsersTake)
                .Select(x => new UserInfoView
                {
                    Id = x.UserId,
                    Name = x.User.UserName,
                    Link = x.User.Link,
                    Avatar = x.User.Avatar
                }).ToArrayAsync();
        }

        public Task<RoleView[]> GetUserRolesAsync(int userId)
        {
            return db.Roles.Where(x => x.Users.Any(y => y.UserId == userId)).Select(x => new RoleView
            {
                Name = x.Name,
                Title = x.Title,
                UsersCount = x.Users.Count,
                IsSuper = x.IsSuper
            }).ToArrayAsync();
        }
    }

    public class RoleView
    {
        public string Name { get; set; }
        public string Title { get; set; }
        public int UsersCount { get; set; }
        public bool IsSuper { get; set; }
    }
}
