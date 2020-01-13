using System;
using System.Linq;
using System.Threading.Tasks;
using LinqToDB;
using SunEngine.Core.DataBase;
using SunEngine.Core.Errors.Exceptions;
using SunEngine.Core.Models;
using SunEngine.Core.Services;
using SunEngine.Core.Utils;

namespace SunEngine.Admin.Managers
{
	public class CategoriesAdminManager : DbService
	{
		private readonly SanitizerService sanitizerService;

		public CategoriesAdminManager(
			DataBaseConnection db,
			SanitizerService sanitizerService) : base(db)
		{
			this.sanitizerService = sanitizerService;
		}


		public async ValueTask CreateCategoryAsync(Category category)
		{
			category.Name = category.Name.Trim();
			category.NameNormalized = Normalizer.Normalize(category.Name);

			ValidateCategory(category);

			category.Token = category.Token?.SetNullIfEmptyTrim();
			category.SubTitle = category.SubTitle?.SetNullIfEmptyTrim();
			category.Icon = category.Icon?.SetNullIfEmptyTrim();
			category.SettingsJson = category.SettingsJson?.MakeJsonText();
			category.LayoutName = category.LayoutName?.SetNullIfEmptyTrim();
			category.MaterialTypeTitle = category.MaterialTypeTitle?.SetNullIfEmptyTrim();
			category.Header = sanitizerService.Sanitize(category.Header?.SetNullIfEmptyTrim());


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


		public async ValueTask UpdateCategoryAsync(Category categoryUpdate)
		{
			if (categoryUpdate == null)
				throw new ArgumentNullException(nameof(categoryUpdate));

			Category category = await db.Categories.FirstOrDefaultAsync(x => x.Id == categoryUpdate.Id);
			if (category == null)
				throw new SunEntityNotFoundException($"No category with {categoryUpdate.Id} id");

			if (categoryUpdate.Id == categoryUpdate.ParentId)
				throw new SunEntityNotUpdatedException(nameof(Category), "Can not set item as self parent");

			var parent = await db.Categories.FirstOrDefaultAsync(x => x.Id == categoryUpdate.ParentId);

			if (parent == null)
				throw new SunParentEntityNotFoundException(nameof(Category), categoryUpdate.ParentId);

			category.Name = categoryUpdate.Name.Trim();
			category.NameNormalized = Normalizer.Normalize(category.Name);
			category.Title = categoryUpdate.Title?.SetNullIfEmptyTrim();

			ValidateCategory(category);

			category.Token = categoryUpdate.Token?.SetNullIfEmptyTrim();
			category.AppendTokenToSubCatsPath = categoryUpdate.AppendTokenToSubCatsPath;
			category.ShowInBreadcrumbs = categoryUpdate.ShowInBreadcrumbs;
			category.SubTitle = categoryUpdate.SubTitle?.SetNullIfEmptyTrim();
			category.Icon = categoryUpdate.Icon?.SetNullIfEmptyTrim();
			category.MaterialTypeTitle = categoryUpdate.MaterialTypeTitle?.SetNullIfEmptyTrim();
			category.Header = sanitizerService.Sanitize(categoryUpdate.Header?.SetNullIfEmptyTrim());
			category.ParentId = parent.Id;
			category.LayoutName = categoryUpdate.LayoutName?.SetNullIfEmptyTrim();
			category.SettingsJson = categoryUpdate.SettingsJson?.MakeJsonText();
			category.IsHidden = categoryUpdate.IsHidden;
			category.IsCacheContent = categoryUpdate.IsCacheContent;
			category.IsMaterialsSubTitleEditable = categoryUpdate.IsMaterialsSubTitleEditable;
			category.IsMaterialsNameEditable = categoryUpdate.IsMaterialsNameEditable;
			category.IsMaterialsContainer = categoryUpdate.IsMaterialsContainer;


			await db.UpdateAsync(category);
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

		public async ValueTask CategoryUp(string name)
		{
			var category = await db.Categories.FirstOrDefaultAsync(x => x.Name == name);
			if (category == null)
				throw new SunEntityNotFoundException(nameof(Category), name, "Name");

			var category2 = await db.Categories
				.Where(x => x.ParentId == category.ParentId && x.SortNumber < category.SortNumber &&
				            x.DeletedDate == null)
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

		public async ValueTask CategoryDown(string name)
		{
			var category = await db.Categories.FirstOrDefaultAsync(x => x.Name == name);
			if (category == null)
				throw new SunEntityNotFoundException(nameof(Category), name, "Name");

			var category2 = await db.Categories
				.Where(x => x.ParentId == category.ParentId && x.SortNumber > category.SortNumber &&
				            x.DeletedDate == null)
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
			return db.Categories.Where(x => x.Name == name).Set(x => x.DeletedDate, x => DateTime.UtcNow).UpdateAsync();
		}

		/*public async Task RemakeAllMaterialsSubTitleAsync(Category category)
		{
		    if (category == null)
		        throw new SunEntityNotFoundException(nameof(category));

		    var materials = await db.Materials.Where(x => x.CategoryId == category.Id).ToListAsync();

		    foreach (var material in materials)
		    {
		        material.SubTitle =
		            generator?.Invoke(new HtmlParser().Parse(material.Text), materialOptions.PreviewLength);
		        await db.Materials.Where(y => y.Id == material.Id).Set(y => y.Preview, y => material.Preview)
		            .UpdateAsync();
		    }
		}*/
	}
}