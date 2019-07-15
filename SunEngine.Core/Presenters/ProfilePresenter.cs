using System;
using System.Linq;
using System.Threading.Tasks;
using LinqToDB;
using SunEngine.Core.Cache.Services;
using SunEngine.Core.DataBase;
using SunEngine.Core.Models;
using SunEngine.Core.Services;

namespace SunEngine.Core.Presenters
{
    public interface IProfilePresenter
    {
        Task<ProfileView> GetProfileAsync(string link, int? viewerUserId);
    }

    public class ProfilePresenter : DbService, IProfilePresenter
    {
        protected readonly IRolesCache RolesCache;
        
        public ProfilePresenter(DataBaseConnection db, IRolesCache rolesCache) : base(db)
        {
            this.RolesCache = rolesCache;
        }
        
        public virtual async Task<ProfileView> GetProfileAsync(string link, int? viewerUserId)
        {
            IQueryable<User> query;
            if (int.TryParse(link, out int id))
                query = db.Users.Where(x => x.Id == id);
            else
                query = db.Users.Where(x => x.Link == link);

            if (viewerUserId.HasValue)
            {
                int adminGroupId = RolesCache.AdminRole.Id;

                var user = await query.Select(x =>
                    new ProfileView
                    {
                        Id = x.Id,
                        Name = x.UserName,
                        Information = x.Information,
                        Link = x.Link,
                        Photo = x.Photo,
                        RegisteredDate = x.RegisteredDate,
                        NoBannable = x.Roles.Any(y => y.RoleId == adminGroupId),
                        HeBannedMe = x.BanList.Any(y => y.UserBanedId == viewerUserId.Value)
                    }).FirstOrDefaultAsync();

                user.IBannedHim = await db.Users.Where(y => y.Id == viewerUserId.Value)
                    .AnyAsync(x => x.BanList.Any(y => y.UserBanedId == user.Id));

                return user;
            }

            return await query.Select(x =>
                new ProfileView
                {
                    Id = x.Id,
                    Name = x.UserName,
                    Information = x.Information,
                    Link = x.Link,
                    Photo = x.Photo,
                    RegisteredDate = x.RegisteredDate,
                    NoBannable = true,
                    HeBannedMe = false,
                    IBannedHim = false
                }).FirstOrDefaultAsync();
        }
    }
    
    

    public class ProfileView
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Information { get; set; }
        public string Link { get; set; }

        public string Photo { get; set; }
        
        public DateTime RegisteredDate { get; set; }

        public bool NoBannable { get; set; }
        public bool HeBannedMe { get; set; }
        public bool IBannedHim { get; set; }
    }
}
