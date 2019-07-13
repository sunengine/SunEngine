using System;
using System.Linq;
using System.Threading.Tasks;
using LinqToDB;
using SunEngine.Core.DataBase;
using SunEngine.Core.Services;

namespace SunEngine.Core.Presenters
{
    public interface ISearchPresenter
    {
        Task<UserInfoView[]> SearchByUsernameOrLink(string searchString);
    }
    
    public class SearchPresenter : DbService, ISearchPresenter
    {
        public SearchPresenter(DataBaseConnection db) : base(db)
        {
        }

        public Task<UserInfoView[]>  SearchByUsernameOrLink(string searchString)
        {
            return db.Users.Where(x => x.UserName.Contains(searchString,StringComparison.OrdinalIgnoreCase)||
                                       x.Link.Contains(searchString,StringComparison.OrdinalIgnoreCase))
                .Select(x => new UserInfoView
            {
                Avatar = x.Avatar,
                Id = x.Id,
                Link = x.Link,
                Name = x.UserName
            }).ToArrayAsync();
        }
    }
    
    public class UserInfoView
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Link { get; set; }
        public string Avatar { get; set; }
    }
}
