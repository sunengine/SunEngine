using System.Linq;
using System.Threading.Tasks;
using LinqToDB;
using SunEngine.Commons.DataBase;
using SunEngine.Commons.Presenters;
using SunEngine.Commons.Security;
using SunEngine.Commons.Services;
using SunEngine.Commons.Utils;

namespace SunEngine.Admin.Presenters
{
    public interface IUserRolesAdminPresenter
    {
        Task<RoleViewModel[]> GetAllRolesAsync();
        Task<UserInfoViewModel[]> GetRoleUsers(string groupName, string userNamePart);
        Task<RoleViewModel[]> GetUserRolesAsync(int userId);
    }

    public class UserRolesAdminPresenter : DbService, IUserRolesAdminPresenter
    {
        private const int MaxUsersTake = 40;
        
        
        public UserRolesAdminPresenter(DataBaseConnection db) : base(db)
        {
        }

        public Task<RoleViewModel[]> GetAllRolesAsync()
        {
            return db.Roles.Where(x=>x.NormalizedName !=  RoleNames.UnregisteredNormalized).Select(x => new RoleViewModel
            {
                Name = x.Name,
                Title = x.Title,
                UsersCount = x.Users.Count,
                IsSuper = x.IsSuper
            }).ToArrayAsync();
        }

        public Task<UserInfoViewModel[]> GetRoleUsers(string roleName, string userNamePart)
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
                .Select(x => new UserInfoViewModel
                {
                    Id = x.UserId,
                    Name = x.User.UserName,
                    Link = x.User.Link,
                    Avatar = x.User.Avatar
                }).ToArrayAsync();
        }

        public Task<RoleViewModel[]> GetUserRolesAsync(int userId)
        {
            return db.Roles.Where(x => x.Users.Any(y => y.UserId == userId)).Select(x => new RoleViewModel
            {
                Name = x.Name,
                Title = x.Title,
                UsersCount = x.Users.Count,
                IsSuper = x.IsSuper
            }).ToArrayAsync();
        }
    }

    public class RoleViewModel
    {
        public string Name { get; set; }
        public string Title { get; set; }
        public int UsersCount { get; set; }
        public bool IsSuper { get; set; }
    }
}