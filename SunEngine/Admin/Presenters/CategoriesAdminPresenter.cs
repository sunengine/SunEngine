using System.Linq;
using System.Threading.Tasks;
using LinqToDB;
using SunEngine.Admin.Controllers;
using SunEngine.DataBase;
using SunEngine.Models;
using SunEngine.Services;

namespace SunEngine.Admin.Presenters
{
    public interface ICategoriesAdminPresenter
    {
        Task<CategoryAdminViewModel> GetCategoryAsync(int id);
        Task<CategoryAdminViewModel> GetCategoryAsync(string name);
    }

    public class CategoriesAdminPresenter : DbService, ICategoriesAdminPresenter
    {
        public CategoriesAdminPresenter(DataBaseConnection db) : base(db)
        {
        }
        
        public Task<CategoryAdminViewModel> GetCategoryAsync(int id)
        {
            var query = db.Categories.Where(x => x.Id == id);
            return GetCategoryByQueryAsync(query);
        }
        
        public Task<CategoryAdminViewModel> GetCategoryAsync(string name)
        {
            var query = db.Categories.Where(x => x.Name == name);
            return GetCategoryByQueryAsync(query);
        }
        
        private Task<CategoryAdminViewModel> GetCategoryByQueryAsync(IQueryable<Category> categoryQuery)
        {
            return categoryQuery.Select(x => new CategoryAdminViewModel
            {
                Id = x.Id,
                Name = x.Name,
                Title = x.Title,
                IsFolder = x.IsFolder,
                Description = x.Description,
                Header = x.Header,
                AreaRoot = x.AreaRoot,
                ParentId = x.ParentId,
                SortNumber = x.SortNumber,
                MaterialsCount = x.Materials.Count,
                IsHidden = x.IsHidden,
                IsDeleted = x.IsDeleted,

            }).FirstOrDefaultAsync();
        }

        
    }
}