using System;
using System.Linq;
using System.Threading.Tasks;
using LinqToDB;
using SunEngine.Commons.DataBase;
using SunEngine.Services;

namespace SunEngine.Presenters
{
    public class MaterialsPresenter : DbService
    {
        public MaterialsPresenter(DataBaseConnection db) : base(db)
        {
        }

        public Task<MaterialViewModel> GetViewModelAsync(int id)
        {
            return db.Materials.Where(x => x.Id == id).Select(x =>
                new MaterialViewModel
                {
                    Id = x.Id,
                    Title = x.Title,
                    AuthorLink = x.Author.Link,
                    AuthorName = x.Author.UserName,
                    AuthorAvatar = x.Author.Avatar,
                    AuthorId = x.Author.Id,
                    PublishDate = x.PublishDate,
                    EditDate = x.EditDate,
                    MessagesCount = x.MessagesCount,
                    Text = x.Text,
                    CategoryName = x.Category.Name.ToLower(),
                    IsDeleted = x.IsDeleted,
                    Tags = x.TagMaterials.OrderBy(y => y.Tag.Name).Select(y => y.Tag.Name).ToArray()
                }
            ).FirstOrDefaultAsync();
        }
    }

    public class MaterialViewModel
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Text { get; set; }
        public string AuthorName { get; set; }
        public int AuthorId { get; set; }
        public string AuthorLink { get; set; }
        public string AuthorAvatar { get; set; }
        public int MessagesCount { get; set; }
        public DateTime PublishDate { get; set; }
        public DateTime? EditDate { get; set; }
        public string CategoryName { get; set; }
        public bool IsDeleted { get; set; }
        public string[] Tags { get; set; }
    }
}