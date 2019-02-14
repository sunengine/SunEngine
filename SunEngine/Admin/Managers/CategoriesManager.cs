using System;
using System.Linq;
using System.Threading.Tasks;
using System.Transactions;
using LinqToDB;
using SunEngine.DataBase;
using SunEngine.Models;
using SunEngine.Services;
using SunEngine.Utils.TextProcess;

namespace SunEngine.Admin.Managers
{
    public class CategoriesManager : DbService
    {
        private readonly Sanitizer sanitizer;
        
        public CategoriesManager(
            DataBaseConnection db,
            Sanitizer sanitizer) : base(db)
        {
            this.sanitizer = sanitizer;
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
            Category сategoryExisted = await db.Categories.FirstOrDefaultAsync(x => x.Id == category.Id);
            if(сategoryExisted == null)
                throw new Exception("No category with " + category.Id + " id");  

            var parent = await db.Categories.FirstOrDefaultAsync(x => x.Id == category.ParentId);
            
            if (!parent.IsFolder)
                throw new Exception("Can not place category in MaterialContainer category type");  
            
            сategoryExisted.Name = category.Name;
            сategoryExisted.Title = category.Title;
            сategoryExisted.Header = sanitizer.Sanitize(category.Header);
            сategoryExisted.ParentId = parent.Id;
            сategoryExisted.IsHidden = category.IsHidden;
            сategoryExisted.IsFolder = category.IsFolder;
            сategoryExisted.AreaRoot = category.AreaRoot;
            
            await db.UpdateAsync(сategoryExisted);
        }
    }
}