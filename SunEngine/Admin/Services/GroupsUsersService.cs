using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using LinqToDB;
using SunEngine.DataBase;
using SunEngine.Managers;
using SunEngine.Services;

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
        
        public Task<UserGroupViewModel[]> GetUserGroupsAsync(int userId)
        {
            return db.UserGroups.Where(x=>x.Users.Any(y=>y.UserId == userId)).Select(x => new UserGroupViewModel
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