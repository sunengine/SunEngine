using System;
using System.Threading.Tasks;
using LinqToDB;
using SunEngine.Commons.DataBase;
using SunEngine.Commons.Models;
using SunEngine.Commons.TextProcess;
using SunEngine.EntityServices;

namespace SunEngine.Services.Admin
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

        public async Task AddCategory(Category category)
        {
            category.Header = sanitizer.Sanitize(category.Header);

            var parent = await db.Categories.FirstOrDefaultAsync(x => x.Id == category.ParentId);
            
            if (!parent.IsFolder)
                throw new Exception("Can not add in MaterialContainer category type");    
                
            await db.InsertAsync(category);
        }
    }
}