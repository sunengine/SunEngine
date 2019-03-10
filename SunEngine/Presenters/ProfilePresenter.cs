using System.Linq;
using System.Threading.Tasks;
using LinqToDB;
using SunEngine.Cache;
using SunEngine.DataBase;
using SunEngine.Models;
using SunEngine.Services;

namespace SunEngine.Presenters
{
    public interface IProfilePresenter
    {
        Task<ProfileViewModel> GetProfileAsync(string link, int? viewerUserId);
    }

    public class ProfilePresenter : DbService, IProfilePresenter
    {
        protected readonly IRolesCache RolesCache;
        
        public ProfilePresenter(DataBaseConnection db, IRolesCache rolesCache) : base(db)
        {
            this.RolesCache = rolesCache;
        }
        
        public virtual async Task<ProfileViewModel> GetProfileAsync(string link, int? viewerUserId)
        {
            IQueryable<User> query;
            if (int.TryParse(link, out int id))
                query = db.Users.Where(x => x.Id == id);
            else
                query = db.Users.Where(x => x.Link == link);

            if (viewerUserId.HasValue)
            {
                int adminGroupId = RolesCache.AllRoles["Admin"].Id;

                var user = await query.Select(x =>
                    new ProfileViewModel
                    {
                        Id = x.Id,
                        Name = x.UserName,
                        Information = x.Information,
                        Link = x.Link,
                        Photo = x.Photo,
                        NoBannable = x.Roles.Any(y => y.RoleId == adminGroupId),
                        HeBannedMe = x.BanList.Any(y => y.UserBanedId == viewerUserId.Value),
                    }).FirstOrDefaultAsync();

                user.IBannedHim = await db.Users.Where(y => y.Id == viewerUserId.Value)
                    .AnyAsync(x => x.BanList.Any(y => y.UserBanedId == user.Id));

                return user;
            }

            return await query.Select(x =>
                new ProfileViewModel
                {
                    Id = x.Id,
                    Name = x.UserName,
                    Information = x.Information,
                    Link = x.Link,
                    Photo = x.Photo,
                    NoBannable = true,
                    HeBannedMe = false,
                    IBannedHim = false
                }).FirstOrDefaultAsync();
        }
    }
    
    public class UserInfoViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Link { get; set; }
        public string Avatar { get; set; }
    }

    public class ProfileViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Information { get; set; }
        public string Link { get; set; }

        public string Photo { get; set; }

        public bool NoBannable { get; set; }
        public bool HeBannedMe { get; set; }
        public bool IBannedHim { get; set; }

        //public string Avatar { get; set; }
    }
}