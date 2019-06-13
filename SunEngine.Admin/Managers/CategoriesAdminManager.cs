using System;
using System.Linq;
using System.Threading.Tasks;
using LinqToDB;
using SunEngine.Core.Cache.Services;
using SunEngine.Core.DataBase;
using SunEngine.Core.Errors;
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


        public async Task CreateCategoryAsync(Category category)
        {
            category.Name = category.Name.Trim();
            category.NameNormalized = Normalizer.Normalize(category.Name);
            category.Title = category.Title;

            ValidateCategory(category);


            category.SubTitle = category.SubTitle?.SetNullIfEmptyTrim();
            category.Icon = category.Icon?.SetNullIfEmptyTrim();
            category.SettingsJson = category.SettingsJson?.SetNullIfEmptyTrim();
            category.LayoutName = category.LayoutName?.SetNullIfEmptyTrim();
            category.MaterialTypeTitle = category.MaterialTypeTitle?.SetNullIfEmptyTrim();
            category.Header = sanitizer.Sanitize(category.Header?.SetNullIfEmptyTrim());

            var parent = await db.Categories.FirstOrDefaultAsync(x => x.Id == category.ParentId);

            if (parent == null)
                throw new SunParentEntityNotFoundException(nameof(Category), category.ParentId);

            using (db.BeginTransaction())
            {
                int id = await db.InsertWithInt32IdentityAsync(category);
                await db.Categories.Where(x => x.Id == id).Set(x => x.SortNumber, id).UpdateAsync();
                category.SortNumber = category.Id = id;
                db.CommitTransaction();
            }
        }

        private void ValidateCategory(Category category)
        {
            if (category == null)
                throw new ArgumentNullException(nameof(category), "Category can not be null");

            if (string.IsNullOrEmpty(category.Name))
                throw new SunModelValidationException(nameof(category), nameof(category.Name));

            if (string.IsNullOrEmpty(category.Title))
                throw new SunModelValidationException(nameof(category), nameof(category.Title));
        }

        public async Task UpdateCategoryAsync(Category categoryUpdate)
        {
            if (categoryUpdate == null)
                throw new ArgumentNullException(nameof(categoryUpdate));

            Category category = await db.Categories.FirstOrDefaultAsync(x => x.Id == categoryUpdate.Id);
            if (category == null)
                throw new SunEntityNotFoundException($"No category with {categoryUpdate.Id} id");

            var parent = await db.Categories.FirstOrDefaultAsync(x => x.Id == categoryUpdate.ParentId);

            if (parent == null)
                throw new SunParentEntityNotFoundException(nameof(Category), categoryUpdate.ParentId);

            category.Name = categoryUpdate.Name.Trim();
            category.NameNormalized = Normalizer.Normalize(category.Name);
            category.Title = categoryUpdate.Title?.SetNullIfEmptyTrim();

            ValidateCategory(category);

            category.SubTitle = categoryUpdate.SubTitle?.SetNullIfEmptyTrim();
            category.Icon = categoryUpdate.Icon?.SetNullIfEmptyTrim();
            category.MaterialTypeTitle = categoryUpdate.MaterialTypeTitle?.SetNullIfEmptyTrim();
            category.Header = sanitizer.Sanitize(categoryUpdate.Header?.SetNullIfEmptyTrim());
            category.SectionTypeId = categoryUpdate.SectionTypeId;
            category.ParentId = parent.Id;
            category.LayoutName = categoryUpdate.LayoutName?.SetNullIfEmptyTrim();
            category.SettingsJson = categoryUpdate.SettingsJson?.SetNullIfEmptyTrim();
            category.IsHidden = categoryUpdate.IsHidden;
            category.IsCacheContent = categoryUpdate.IsCacheContent;
            category.IsMaterialsContainer = categoryUpdate.IsMaterialsContainer;


            await db.UpdateAsync(category);
        }

        public async Task CategoryUp(string name)
        {
            var category = await db.Categories.FirstOrDefaultAsync(x => x.Name == name);
            if (category == null)
                throw new SunEntityNotFoundException(nameof(Category), name, "Name");

            var category2 = await db.Categories
                .Where(x => x.ParentId == category.ParentId && x.SortNumber < category.SortNumber && !x.IsDeleted)
                .OrderByDescending(x => x.SortNumber).FirstOrDefaultAsync();

            if (category2 == null)
                throw new SunEntityNotFoundException(nameof(Category), "Previous category not found");

            using (db.BeginTransaction())
            {
                int rowsUpdated = 0;

                rowsUpdated += await db.Categories.Where(x => x.Id == category.Id)
                    .Set(x => x.SortNumber, 0)
                    .UpdateAsync();
                
                rowsUpdated += await db.Categories.Where(x => x.Id == category2.Id)
                    .Set(x => x.SortNumber, category.SortNumber)
                    .UpdateAsync();
                
                rowsUpdated += await db.Categories.Where(x => x.Id == category.Id)
                    .Set(x => x.SortNumber, category2.SortNumber)
                    .UpdateAsync();

                if (rowsUpdated != 3)
                    throw new SunEntityNotUpdatedException(nameof(Category), "change position of 2 Categories");


                db.CommitTransaction();
            }
        }

        public async Task CategoryDown(string name)
        {
            var category = await db.Categories.FirstOrDefaultAsync(x => x.Name == name);
            if (category == null)
                throw new SunEntityNotFoundException(nameof(Category), name, "Name");

            var category2 = await db.Categories
                .Where(x => x.ParentId == category.ParentId && x.SortNumber > category.SortNumber && !x.IsDeleted)
                .OrderBy(x => x.SortNumber).FirstOrDefaultAsync();

            if (category2 == null)
                throw new SunEntityNotFoundException(nameof(Category), "Next category not found");

            using (db.BeginTransaction())
            {
                int rowsUpdated = 0;

                rowsUpdated += await db.Categories.Where(x => x.Id == category.Id)
                    .Set(x => x.SortNumber, 0)
                    .UpdateAsync();
                
                rowsUpdated += await db.Categories.Where(x => x.Id == category2.Id)
                    .Set(x => x.SortNumber, category.SortNumber)
                    .UpdateAsync();
                
                rowsUpdated += await db.Categories.Where(x => x.Id == category.Id)
                    .Set(x => x.SortNumber, category2.SortNumber)
                    .UpdateAsync();

                if (rowsUpdated != 3)
                    throw new SunEntityNotUpdatedException(nameof(Category), "change position of 2 Categories");


                db.CommitTransaction();
            }
        }

        public Task CategoryMoveToTrashAsync(string name)
        {
            return db.Categories.Where(x => x.Name == name).Set(x => x.IsDeleted, x => true).UpdateAsync();
        }

        public Task<SectionType> GetSectionTypeByNameAsync(string name)
        {
            return db.SectionTypes.FirstOrDefaultAsync(x => x.Name == name);
        }
    }
}
