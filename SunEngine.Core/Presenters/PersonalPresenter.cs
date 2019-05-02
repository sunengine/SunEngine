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

        public virtual Task<SunUserInfoView> GetMyUserInfoAsync(int id)
        {
            return db.Users.Where(x => x.Id == id).Select(x =>
                new SunUserInfoView
                {
                    Photo = x.Photo,
                    Avatar = x.Avatar,
                    Link = x.Link
                }).FirstOrDefaultAsync();
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
            return db.UserBanedUnits.Where(x => x.UserId == userId).OrderBy(x=>x.UserBaned.UserName).Select(x => 
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
        public string Photo { get; set; }
        public string Avatar { get; set; }
        public string Link { get; set; }
    }
}