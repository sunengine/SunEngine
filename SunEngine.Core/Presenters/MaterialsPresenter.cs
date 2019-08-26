using System;
using System.Linq;
using System.Threading.Tasks;
using LinqToDB;
using Newtonsoft.Json.Linq;
using SunEngine.Core.DataBase;
using SunEngine.Core.Models.Materials;
using SunEngine.Core.Services;

namespace SunEngine.Core.Presenters
{
    public interface IMaterialsPresenter
    {
        Task<MaterialView> GetAsync(int id);
        Task<MaterialView> GetAsync(string name);
    }

    public class MaterialsPresenter : DbService, IMaterialsPresenter
    {
        public MaterialsPresenter(DataBaseConnection db) : base(db)
        {
        }

        public Task<MaterialView> GetAsync(int id)
        {
            var query = db.Materials.Where(x => x.Id == id);
            return GetAsync(query);
        }

        public Task<MaterialView> GetAsync(string name)
        {
            var query = db.Materials.Where(x => x.Name == name);
            return GetAsync(query);
        }

        protected virtual Task<MaterialView> GetAsync(IQueryable<Material> query)
        {
            return query.Select(x =>
                new MaterialView
                {
                    Id = x.Id,
                    Name = x.Name,
                    Title = x.Title,
                    SubTitle = x.SubTitle,
                    AuthorLink = x.Author.Link,
                    AuthorName = x.Author.UserName,
                    AuthorAvatar = x.Author.Avatar,
                    AuthorId = x.Author.Id,
                    PublishDate = x.PublishDate,
                    EditDate = x.EditDate,
                    CommentsCount = x.CommentsCount,
                    Text = x.Text,
                    CategoryName = x.Category.Name,
                    IsHidden = x.IsHidden,
                    IsCommentsBlocked = x.IsCommentsBlocked,
                    DeletedDate = x.DeletedDate,
                    Tags = x.TagMaterials.OrderBy(y => y.Tag.Name).Select(y => y.Tag.Name).ToArray(),
                    VisitsCount = x.VisitsCount,
                    SettingsJson = x.SettingsJson
                }
            ).FirstOrDefaultAsync();
        }
    }

    public class MaterialView
    {
        public string Name { get; set; }
        public int Id { get; set; }
        public string Title { get; set; }
        public string SubTitle { get; set; }
        public string Text { get; set; }
        public string AuthorName { get; set; }
        public int AuthorId { get; set; }
        public string AuthorLink { get; set; }
        public string AuthorAvatar { get; set; }
        public int CommentsCount { get; set; }
        public DateTime PublishDate { get; set; }
        public DateTime? EditDate { get; set; }
        public string CategoryName { get; set; }
        public bool IsCommentsBlocked { get; set; }
        public bool IsHidden { get; set; }
        public DateTime? DeletedDate { get; set; }
        public string[] Tags { get; set; }
        public int VisitsCount { get; set; }
        public string SettingsJson { get; set; }
    }
}
