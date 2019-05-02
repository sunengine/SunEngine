using System;
using System.Linq;
using System.Threading.Tasks;
using System.Transactions;
using LinqToDB;
using SunEngine.Core.Cache.Services;
using SunEngine.Core.DataBase;
using SunEngine.Core.Models;
using SunEngine.Core.Services;
using SunEngine.Core.Utils;
using SunEngine.Core.Utils.TextProcess;

namespace SunEngine.Admin.Managers
{
    public class CategoriesAdminManager : DbService
    {
        private readonly Sanitizer sanitizer;
        private readonly ICategoriesCache categoriesCache;

        
        public CategoriesAdminManager(
            DataBaseConnection db,
            ICategoriesCache categoriesCache,
            Sanitizer sanitizer) : base(db)
        {
            this.sanitizer = sanitizer;
            this.categoriesCache = categoriesCache;
        }

        public Task<SectionType> GetSectionTypeByNameAsync(string name)
        {
            return db.SectionTypes.FirstOrDefaultAsync(x => x.Name == name);
        }
        
        public async Task CreateCategoryAsync(Category category)
        {
            if(category == null)
                throw new ArgumentNullException("Category can not be null");
            
            category.Header = sanitizer.Sanitize(category.Header);
            category.NameNormalized = Normalizer.Normalize(category.Name);

            var parent = await db.Categories.FirstOrDefaultAsync(x => x.Id == category.ParentId);
            
            if (parent == null)
                throw new ParentCategoryNotFoundByIdException(category.ParentId);
            
            using (var transaction = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
            {
                int id = await db.InsertWithInt32IdentityAsync(category);
                await db.Categories.Where(x => x.Id == id).Set(x => x.SortNumber, id).UpdateAsync();
            }
        }
        
        
        public async Task UpdateCategoryAsync(Category categoryUpdate)
        {
            if(categoryUpdate == null)
                throw new ArgumentNullException("Category can not be null");
            
            Category category = await db.Categories.FirstOrDefaultAsync(x => x.Id == categoryUpdate.Id);
            if(category == null)
                throw new Exception("No category with " + categoryUpdate.Id + " id");  

            var parent = await db.Categories.FirstOrDefaultAsync(x => x.Id == categoryUpdate.ParentId);

            if (parent == null)
                throw new ParentCategoryNotFoundByIdException(categoryUpdate.ParentId);

            category.Name = categoryUpdate.Name;
            category.Title = categoryUpdate.Title;
            category.Header = sanitizer.Sanitize(categoryUpdate.Header);
            category.Description = categoryUpdate.Description;
            category.ParentId = parent.Id;
            category.IsHidden = categoryUpdate.IsHidden;
            category.IsCacheContent = categoryUpdate.IsCacheContent;
            category.IsMaterialsContainer = categoryUpdate.IsMaterialsContainer;
            category.AppendUrlToken = categoryUpdate.AppendUrlToken;
            category.SectionTypeId = categoryUpdate.SectionTypeId;
            
            await db.UpdateAsync(category);
        }
        
        public async Task<ServiceResult> CategoryUp(string name)
        {
            using (var transaction = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
            {
                var category = await db.Categories.FirstOrDefaultAsync(x => x.Name == name);
                if (category == null)
                    return ServiceResult.BadResult();

                var category2 = await db.Categories
                    .Where(x => x.ParentId == category.ParentId && x.SortNumber < category.SortNumber)
                    .OrderByDescending(x => x.SortNumber).FirstOrDefaultAsync();

                if (category2 == null)
                    return ServiceResult.BadResult();

                await db.Categories.Where(x => x.Id == category.Id).Set(x => x.SortNumber, category2.SortNumber)
                    .UpdateAsync();
                await db.Categories.Where(x => x.Id == category2.Id).Set(x => x.SortNumber, category.SortNumber)
                    .UpdateAsync();

                transaction.Complete();
            }

            return ServiceResult.OkResult();
        }

        public async Task<ServiceResult> CategoryDown(string name)
        {
            using (var transaction = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
            {
                var category = await db.Categories.FirstOrDefaultAsync(x => x.Name == name);
                if (category == null)
                    return ServiceResult.BadResult();

                var category2 = await db.Categories
                    .Where(x => x.ParentId == category.ParentId && x.SortNumber > category.SortNumber)
                    .OrderBy(x => x.SortNumber).FirstOrDefaultAsync();

                if (category2 == null)
                    return ServiceResult.BadResult();

                await db.Categories.Where(x => x.Id == category.Id).Set(x => x.SortNumber, category2.SortNumber)
                    .UpdateAsync();
                await db.Categories.Where(x => x.Id == category2.Id).Set(x => x.SortNumber, category.SortNumber)
                    .UpdateAsync();

                transaction.Complete();
            }
            
            return ServiceResult.OkResult();
        }

        public Task CategoryMoveToTrashAsync(string name)
        {
            return db.Categories.Where(x => x.Name == name).Set(x => x.IsDeleted, x => true).UpdateAsync();
        }
    }

    public class ParentCategoryNotFoundByIdException : Exception
    {
        public ParentCategoryNotFoundByIdException(int? parentId) : base($"Parent category (id:{parentId}) not found. Can not add category without parent")
        {               
        }
    }
}