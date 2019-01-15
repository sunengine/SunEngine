using System.Linq;
using System.Threading.Tasks;
using SunEngine.Commons.DataBase;
using SunEngine.Commons.PagedList;
using SunEngine.Controllers;

namespace SunEngine.EntityServices
{
    public class BlogService : DbService
    {
        public BlogService(DataBaseConnection db) : base(db)
        {
        }

        public Task<IPagedList<PostViewModel>> GetPostsAsync(int categoryId, int page,int pageSize)
        {
            return db.MaterialsNotDeleted.GetPagedListAsync(
                x => new PostViewModel
                {
                    Id = x.Id,
                    Title = x.Title,
                    Preview = x.Preview,
                    MessagesCount = x.MessagesCount,
                    AuthorName = x.Author.UserName,
                    AuthorLink = x.Author.Link,
                    AuthorAvatar = x.Author.Avatar,
                    PublishDate = x.PublishDate,
                    CategoryName = x.Category.Name.ToLower(),
                    HasMoreText = x.Text.Length != x.Preview.Length
                },
                x => x.CategoryId == categoryId,
                x => x.OrderByDescending(y => y.LastActivity),
                page,
                pageSize);
        }
        
    }
}