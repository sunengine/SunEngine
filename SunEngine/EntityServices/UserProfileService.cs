using System.Linq;
using System.Threading.Tasks;
using LinqToDB;
using SunEngine.Commons.DataBase;
using SunEngine.Commons.Models;

namespace SunEngine.EntityServices
{
    public class UserProfileService : DbService
    {
        public UserProfileService(DataBaseConnection db) : base(db)
        {
        }


        public Task<ProfileViewModel> GetProfileAsync(string link)
        {
            IQueryable<User> query;
            if (int.TryParse(link, out int id))
            {
                query = db.Users.Where(x => x.Id == id);
            }
            else
            {
                query = db.Users.Where(x => x.Link == link);
            }

            return query.Select(x =>
                new ProfileViewModel
                {
                    Id = x.Id,
                    Name = x.UserName,
                    Information = x.Information,
                    Link = x.Link,
                    Photo = x.Photo
                }).FirstOrDefaultAsync();
        }
    }

    public class ProfileViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Information { get; set; }
        public string Link { get; set; }

        public string Photo { get; set; }
        //public string Avatar { get; set; }
    }
}