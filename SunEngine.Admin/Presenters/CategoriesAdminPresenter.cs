using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LinqToDB;
using SunEngine.Core.DataBase;
using SunEngine.Core.Models;
using SunEngine.Core.Services;

namespace SunEngine.Admin.Presenters
{
    public interface ICategoriesAdminPresenter
    {
        Task<CategoryAdminView> GetCategoryAsync(int id);
        Task<CategoryAdminView> GetCategoryAsync(string name);
        Task<CategoryAdminView> GetAllCategoriesAsync();
    }

    public class CategoriesAdminPresenter : DbService, ICategoriesAdminPresenter
    {
        public CategoriesAdminPresenter(DataBaseConnection db) : base(db)
        {
        }

        public Task<CategoryAdminView> GetCategoryAsync(int id)
        {
            var query = db.Categories.Where(x => x.Id == id);
            return GetCategoryByQueryAsync(query);
        }

        public Task<CategoryAdminView> GetCategoryAsync(string name)
        {
            var query = db.Categories.Where(x => x.Name == name);
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
                IsMaterialsNameEditable = x.IsMaterialsNameEditable,
                MaterialsSubTitleInputType = x.MaterialsSubTitleInputType,
                MaterialsPreviewGeneratorName = x.MaterialsPreviewGeneratorName,
                SubTitle = x.SubTitle,
                Icon = x.Icon,
                Header = x.Header,
                LayoutName = x.LayoutName,
                ParentId = x.ParentId,
                SortNumber = x.SortNumber,
                MaterialsCount = x.Materials.Count,
                IsHidden = x.IsHidden,
                IsCacheContent = x.IsCacheContent,
                DeletedDate = x.DeletedDate,
                SettingsJson = x.SettingsJson
            }).FirstOrDefaultAsync();
        }

        public async Task<CategoryAdminView> GetAllCategoriesAsync()
        {
            var categories = await db.Categories.Where(x => x.DeletedDate == null)
                .OrderBy(x => x.SortNumber).Select(x => new CategoryAdminView
                {
                    Id = x.Id,
                    Name = x.Name,
                    Title = x.Title,
                    SubTitle = x.SubTitle,
                    Icon = x.Icon,
                    IsMaterialsContainer = x.IsMaterialsContainer,
                    Header = x.Header,
                    LayoutName = x.LayoutName,
                    MaterialsPreviewGeneratorName = x.MaterialsPreviewGeneratorName,
                    IsMaterialsNameEditable = x.IsMaterialsNameEditable,
                    MaterialsSubTitleInputType = x.MaterialsSubTitleInputType,
                    ParentId = x.ParentId,
                    SortNumber = x.SortNumber,
                    MaterialsCount = x.Materials.Count,
                    IsHidden = x.IsHidden,
                    IsCacheContent = x.IsCacheContent,
                    DeletedDate = x.DeletedDate,
                    SettingsJson = x.SettingsJson
                }).ToDictionaryAsync(x => x.Id);

            CategoryAdminView root = null;

            foreach (var category in categories.Values)
            {
                if (!category.ParentId.HasValue)
                {
                    root = category;
                    continue;
                }

                if (!categories.ContainsKey(category.ParentId.Value))
                    continue;

                var parent = categories[category.ParentId.Value];

                if (parent.SubCategories == null)
                    parent.SubCategories = new List<CategoryAdminView>();

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

        public string SubTitle { get; set; }

        public string Icon { get; set; }

        public bool IsMaterialsContainer { get; set; }

        public string Header { get; set; }

        public string LayoutName { get; set; }

        public MaterialsSubTitleInputType MaterialsSubTitleInputType { get; set; }

        public bool IsMaterialsNameEditable { get; set; }

        public string MaterialsPreviewGeneratorName { get; set; }
        
        public int? ParentId { get; set; }

        public IList<CategoryAdminView> SubCategories { get; set; }

        public int SortNumber { get; set; }

        public int MaterialsCount { get; set; }
        
        public bool IsHidden { get; set; }

        public bool IsCacheContent { get; set; }
        
        public DateTime? DeletedDate { get; set; }
        
        public string SettingsJson { get; set; }
    }
}
