using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LinqToDB;
using SunEngine.DataBase;
using SunEngine.Models;
using SunEngine.Services;

namespace SunEngine.Admin.Presenters
{
    public interface ICategoriesAdminPresenter
    {
        Task<CategoryAdminViewModel> GetCategoryAsync(int id);
        Task<CategoryAdminViewModel> GetCategoryAsync(string name);
        Task<CategoryAdminViewModel> GetAllCategoriesAsync();
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

        public async Task<CategoryAdminViewModel> GetAllCategoriesAsync()
        {
            var categories = await db.Categories.OrderBy(x => x.SortNumber).Select(x => new CategoryAdminViewModel
            {
                Id = x.Id,
                Name = x.Name,
                Title = x.Title,
                IsFolder = x.IsFolder,
                IsMaterialsContainer = x.IsMaterialsContainer,
                Description = x.Description,
                Header = x.Header,
                AreaRoot = x.AreaRoot,
                ParentId = x.ParentId,
                SortNumber = x.SortNumber,
                MaterialsCount = x.Materials.Count,
                IsHidden = x.IsHidden,
                IsDeleted = x.IsDeleted
            }).ToDictionaryAsync(x => x.Id);

            CategoryAdminViewModel root = null;

            foreach (var category in categories.Values)
            {
                if (!category.ParentId.HasValue)
                {
                    root = category;
                    continue;
                }

                var parent = categories[category.ParentId.Value];
                if (parent.SubCategories == null) parent.SubCategories = new List<CategoryAdminViewModel>();

                parent.SubCategories.Add(category);
            }

            return root;
        }
    }

    public class CategoryAdminViewModel
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Title { get; set; }

        public bool IsFolder { get; set; }

        public bool IsMaterialsContainer { get; set; }

        public string Description { get; set; }

        public string Header { get; set; }

        public bool AreaRoot { get; set; }

        public int? ParentId { get; set; }

        public IList<CategoryAdminViewModel> SubCategories { get; set; }

        public int SortNumber { get; set; }

        public int MaterialsCount { get; set; }

        public bool IsDeleted { get; set; }

        public bool IsHidden { get; set; }
    }
}