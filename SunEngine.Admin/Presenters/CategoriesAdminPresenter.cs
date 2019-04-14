using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LinqToDB;
using SunEngine.Commons.DataBase;
using SunEngine.Commons.Models;
using SunEngine.Commons.Services;

namespace SunEngine.Admin.Presenters
{
    public interface ICategoriesAdminPresenter
    {
        Task<SectionType[]> GetAllSectionTypesAsync();
        Task<CategoryAdminView> GetCategoryAsync(int id);
        Task<CategoryAdminView> GetCategoryAsync(string name);
        Task<CategoryAdminView> GetAllCategoriesAsync();
    }

    public class CategoriesAdminPresenter : DbService, ICategoriesAdminPresenter
    {
        public CategoriesAdminPresenter(DataBaseConnection db) : base(db)
        {
        }

        public Task<SectionType[]> GetAllSectionTypesAsync()
        {
            return db.SectionTypes.OrderBy(x => x.Id).ToArrayAsync();
        }

        public Task<CategoryAdminView> GetCategoryAsync(int id)
        {
            var query = db.Categories.LoadWith(x => x.SectionType).Where(x => x.Id == id);
            return GetCategoryByQueryAsync(query);
        }

        public Task<CategoryAdminView> GetCategoryAsync(string name)
        {
            var query = db.Categories.LoadWith(x => x.SectionType).Where(x => x.Name == name);
            return GetCategoryByQueryAsync(query);
        }

        private Task<CategoryAdminView> GetCategoryByQueryAsync(IQueryable<Category> categoryQuery)
        {
            return categoryQuery.Select(x => new CategoryAdminView
            {
                Id = x.Id,
                Name = x.Name,
                Title = x.Title,
                IsMaterialsContainer = x.IsMaterialsContainer,
                Description = x.Description,
                Header = x.Header,
                SectionTypeName = x.SectionType != null ? x.SectionType.Name : null,
                SectionType = x.SectionType,
                AppendUrlToken = x.AppendUrlToken,
                ParentId = x.ParentId,
                SortNumber = x.SortNumber,
                MaterialsCount = x.Materials.Count,
                IsHidden = x.IsHidden,
                IsDeleted = x.IsDeleted,
                IsCacheContent = x.IsCacheContent
            }).FirstOrDefaultAsync();
        }

        public async Task<CategoryAdminView> GetAllCategoriesAsync()
        {
            var categories = await db.Categories.LoadWith(x => x.SectionType).Where(x=>!x.IsDeleted)
                .OrderBy(x => x.SortNumber).Select(x => new CategoryAdminView
                {
                    Id = x.Id,
                    Name = x.Name,
                    Title = x.Title,
                    IsMaterialsContainer = x.IsMaterialsContainer,
                    Description = x.Description,
                    Header = x.Header,
                    SectionTypeName = x.SectionType != null ? x.SectionType.Name : null,
                    SectionType = x.SectionType,
                    AppendUrlToken = x.AppendUrlToken,
                    ParentId = x.ParentId,
                    SortNumber = x.SortNumber,
                    MaterialsCount = x.Materials.Count,
                    IsHidden = x.IsHidden,
                    IsDeleted = x.IsDeleted,
                    IsCacheContent = x.IsCacheContent
                }).ToDictionaryAsync(x => x.Id);

            CategoryAdminView root = null;

            foreach (var category in categories.Values)
            {
                if (!category.ParentId.HasValue)
                {
                    root = category;
                    continue;
                }

                var parent = categories[category.ParentId.Value];
                if (parent.SubCategories == null) parent.SubCategories = new List<CategoryAdminView>();

                parent.SubCategories.Add(category);
            }

            return root;
        }
    }

    public class CategoryAdminView
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Title { get; set; }

        public bool IsMaterialsContainer { get; set; }

        public string Description { get; set; }

        public string Header { get; set; }

        public string SectionTypeName { get; set; }

        public SectionType SectionType { get; set; }

        public bool AppendUrlToken { get; set; }

        public int? ParentId { get; set; }

        public IList<CategoryAdminView> SubCategories { get; set; }

        public int SortNumber { get; set; }

        public int MaterialsCount { get; set; }

        public bool IsDeleted { get; set; }

        public bool IsHidden { get; set; }
        
        public bool IsCacheContent { get; set; }
    }
}