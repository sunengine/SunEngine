using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using LinqToDB;
using SunEngine.DataBase;
using SunEngine.Managers;
using SunEngine.Presenters;
using SunEngine.Services;
using SunEngine.Utils;

namespace SunEngine.Admin.Services
{
    public class GroupsUsersService : DbService
    {
        public GroupsUsersService(DataBaseConnection db) : base(db)
        {
        }

        public Task<UserGroupViewModel[]> GetAllUserGroupsAsync()
        {
            return db.UserGroups.Select(x => new UserGroupViewModel
            {
                Name = x.Name,
                Title = x.Title,
                UsersCount = x.Users.Count,
                IsSuper = x.IsSuper
            }).ToArrayAsync();
        }

        public Task<UserInfoViewModel[]> GetGroupUsers(string groupName, string userNameStart)
        {
            var normalizedGroupName = Normalizer.Singleton.Normalize(groupName);
            var query = db.UserToGroups.Where(x => x.UserGroup.NormalizedName == normalizedGroupName);

            if (userNameStart != null)
            {
                userNameStart = userNameStart.Trim().ToLower();
                if (!string.IsNullOrEmpty(userNameStart))
                {
                    query = query.Where(x => x.User.UserName.ToLower().StartsWith(userNameStart));
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

        public Task<UserGroupViewModel[]> GetUserGroupsAsync(int userId)
        {
            return db.UserGroups.Where(x => x.Users.Any(y => y.UserId == userId)).Select(x => new UserGroupViewModel
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