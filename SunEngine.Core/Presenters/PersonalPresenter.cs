using System.Linq;
using System.Threading.Tasks;
using LinqToDB;
using SunEngine.Core.DataBase;
using SunEngine.Core.Services;

namespace SunEngine.Core.Presenters
{
    public interface IPersonalPresenter
    {
        Task<SunUserInfoView> GetMyUserInfoAsync(int id);
        Task<SunProfileInformationView> GetMyProfileInformationAsync(int id);
        Task<UserInfoView[]> GetBanListAsync(int userId);
    }

    public class PersonalPresenter : DbService, IPersonalPresenter
    {
        public PersonalPresenter(DataBaseConnection db) : base(db)
        {
        }

        public virtual async Task<SunUserInfoView> GetMyUserInfoAsync(int id)
        {
            var user = await db.Users.Where(x => x.Id == id).Select(x =>
                new
                {
                    sunUserInfoView =
                        new SunUserInfoView
                        {
                            Id = x.Id,
                            Name = x.UserName,
                            Photo = x.Photo,
                            Avatar = x.Avatar,
                            Link = x.Link,
                        },
                    rolesIds = x.Roles.Select(t => t.RoleId)
                }).FirstOrDefaultAsync();

            user.sunUserInfoView.Roles =
                db.Roles.Where(x => user.rolesIds.Contains(x.Id)).Select(x => x.Name).ToArray();

            return user.sunUserInfoView;
        }

        public virtual Task<SunProfileInformationView> GetMyProfileInformationAsync(int id)
        {
            return db.Users.Where(x => x.Id == id).Select(x =>
                new SunProfileInformationView
                {
                    Information = x.Information
                }).FirstOrDefaultAsync();
        }

        public virtual Task<UserInfoView[]> GetBanListAsync(int userId)
        {
            return db.UserBanedUnits.Where(x => x.UserId == userId).OrderBy(x => x.UserBaned.UserName).Select(x =>
                new UserInfoView
                {
                    Id = x.UserBaned.Id,
                    Name = x.UserBaned.UserName,
                    Link = x.UserBaned.Link
                }).ToArrayAsync();
        }
    }

    public class SunProfileInformationView
    {
        public string Information { get; set; }
    }

    public class SunUserInfoView
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string[] Roles { get; set; }
        public string Photo { get; set; }
        public string Avatar { get; set; }
        public string Link { get; set; }
    }
}
