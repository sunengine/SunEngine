using System;
using System.Linq;
using System.Threading.Tasks;
using LinqToDB;
using SunEngine.Core.DataBase;
using SunEngine.Core.Models;
using SunEngine.Core.Services;

namespace SunEngine.Core.Presenters
{
    public interface ISearchPresenter
    {
        Task<UserSearchInfoView[]> SearchByUsernameAsync(string searchString);
    }
    
    public class SearchPresenter : DbService, ISearchPresenter
    {
        public SearchPresenter(DataBaseConnection db) : base(db)
        {
        }

        public async Task<UserSearchInfoView[]>  SearchByUsernameAsync(string searchString)
        {
            return await db.Users.Where(x => x.UserName.Contains(searchString,StringComparison.OrdinalIgnoreCase)).Select(x => new UserSearchInfoView()
            {
                Avatar = x.Avatar,
                Id = x.Id,
                Link = x.Link,
                Name = x.UserName
            }).ToArrayAsync();
        } 
    }
    
    public class UserSearchInfoView
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Avatar { get; set; }
        public string Link { get; set; }
    }
}