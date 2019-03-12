using System;
using System.Linq;
using System.Threading.Tasks;
using System.Transactions;
using LinqToDB;
using SunEngine.Cache;
using SunEngine.DataBase;
using SunEngine.Models;
using SunEngine.Services;
using SunEngine.Utils;
using SunEngine.Utils.TextProcess;

namespace SunEngine.Admin.Managers
{
    public class CategoriesManager : DbService
    {
        private readonly Sanitizer sanitizer;
        private readonly ICategoriesCache categoriesCache;

        
        public CategoriesManager(
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
        
        public async Task AddCategoryAsync(Category category)
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
        
        public async Task EditCategoryAsync(Category category)
        {
            if(category == null)
                throw new ArgumentNullException("Category can not be null");
            
            Category сategoryExisted = await db.Categories.FirstOrDefaultAsync(x => x.Id == category.Id);
            if(сategoryExisted == null)
                throw new Exception("No category with " + category.Id + " id");  

            var parent = await db.Categories.FirstOrDefaultAsync(x => x.Id == category.ParentId);

            if (parent == null)
                throw new ParentCategoryNotFoundByIdException(category.ParentId);

            сategoryExisted.Name = category.Name;
            сategoryExisted.Title = category.Title;
            сategoryExisted.Header = sanitizer.Sanitize(category.Header);
            сategoryExisted.Description = category.Description;
            сategoryExisted.ParentId = parent.Id;
            сategoryExisted.IsHidden = category.IsHidden;
            сategoryExisted.IsCacheContent = category.IsCacheContent;
            сategoryExisted.IsMaterialsContainer = category.IsMaterialsContainer;
            сategoryExisted.AppendUrlToken = category.AppendUrlToken;
            сategoryExisted.SectionTypeId = category.SectionTypeId;
            
            await db.UpdateAsync(сategoryExisted);
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