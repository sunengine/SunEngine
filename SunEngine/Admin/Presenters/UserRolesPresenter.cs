using System.Linq;
using System.Threading.Tasks;
using LinqToDB;
using SunEngine.DataBase;
using SunEngine.Presenters;
using SunEngine.Security;
using SunEngine.Services;
using SunEngine.Utils;

namespace SunEngine.Admin.Presenters
{
    public interface IUserRolesPresenter
    {
        Task<UserGroupViewModel[]> GetAllRolesAsync();
        Task<UserInfoViewModel[]> GetRoleUsers(string groupName, string userNamePart);
        Task<UserGroupViewModel[]> GetUserRolesAsync(int userId);
    }

    public class UserRolesPresenter : DbService, IUserRolesPresenter
    {
        public UserRolesPresenter(DataBaseConnection db) : base(db)
        {
        }

        public Task<UserGroupViewModel[]> GetAllRolesAsync()
        {
            return db.Roles.Where(x=>x.NormalizedName !=  RoleNames.UnregisteredNormalized).Select(x => new UserGroupViewModel
            {
                Name = x.Name,
                Title = x.Title,
                UsersCount = x.Users.Count,
                IsSuper = x.IsSuper
            }).ToArrayAsync();
        }

        public Task<UserInfoViewModel[]> GetRoleUsers(string roleName, string userNamePart)
        {
            var normalizedGroupName = FieldNormalizer.Normalize(roleName);
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
                .Take(40)
                .Select(x => new UserInfoViewModel
                {
                    Id = x.UserId,
                    Name = x.User.UserName,
                    Link = x.User.Link,
                    Avatar = x.User.Avatar
                }).ToArrayAsync();
        }

        public Task<UserGroupViewModel[]> GetUserRolesAsync(int userId)
        {
            return db.Roles.Where(x => x.Users.Any(y => y.UserId == userId)).Select(x => new UserGroupViewModel
            {
                Name = x.Name,
                Title = x.Title,
                UsersCount = x.Users.Count,
                IsSuper = x.IsSuper
            }).ToArrayAsync();
        }
    }

    public class UserGroupViewModel
    {
        public string Name { get; set; }
        public string Title { get; set; }
        public int UsersCount { get; set; }
        public bool IsSuper { get; set; }
    }
}