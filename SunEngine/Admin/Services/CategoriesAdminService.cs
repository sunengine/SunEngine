using System;
using System.Linq;
using System.Threading.Tasks;
using System.Transactions;
using LinqToDB;
using SunEngine.Admin.Controllers;
using SunEngine.DataBase;
using SunEngine.Models;
using SunEngine.Services;
using SunEngine.Utils.TextProcess;

namespace SunEngine.Admin.Services
{
    public class CategoriesAdminService : DbService
    {
        private readonly Sanitizer sanitizer;
        
        public CategoriesAdminService(
            DataBaseConnection db,
            Sanitizer sanitizer) : base(db)
        {
            this.sanitizer = sanitizer;
        }

        public Task<CategoryAdminViewModel> GetCategoryAsync(int id)
        {
            return db.Categories.Where(x => x.Id == id).Select(x => new CategoryAdminViewModel
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
        
        public Task<CategoryAdminViewModel> GetCategoryAsync(string name)
        {
            return db.Categories.Where(x => x.Name == name).Select(x => new CategoryAdminViewModel
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
                IsDeleted = x.IsDeleted,

            }).FirstOrDefaultAsync();
        }


        public async Task AddCategoryAsync(Category category)
        {
            category.Header = sanitizer.Sanitize(category.Header);

            var parent = await db.Categories.FirstOrDefaultAsync(x => x.Id == category.ParentId);
            
            if (!parent.IsFolder)
                throw new Exception("Can not add in MaterialContainer category type");

            using (var transaction = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
            {
                int id = await db.InsertWithInt32IdentityAsync(category);
                await db.Categories.Where(x => x.Id == id).Set(x => x.SortNumber, id).UpdateAsync();
            }
        }
        
        public async Task EditCategoryAsync(Category category)
        {
            Category oldCategory = await db.Categories.FirstOrDefaultAsync(x => x.Id == category.Id);
            if(oldCategory == null)
                throw new Exception("No category with " + category.Id + " id");  

            var parent = await db.Categories.FirstOrDefaultAsync(x => x.Id == category.ParentId);
            
            if (!parent.IsFolder)
                throw new Exception("Can not place category in MaterialContainer category type");  
            
            oldCategory.Name = category.Name;
            oldCategory.Title = category.Title;
            oldCategory.Header = sanitizer.Sanitize(category.Header);
            oldCategory.ParentId = parent.Id;
            oldCategory.IsHidden = category.IsHidden;
            oldCategory.IsFolder = category.IsFolder;
            oldCategory.AreaRoot = category.AreaRoot;
            
            await db.UpdateAsync(oldCategory);
        }
    }
}