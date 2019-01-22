using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Transactions;
using LinqToDB;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using SunEngine.Commons.DataBase;
using SunEngine.Commons.Models;
using SunEngine.Stores;

namespace SunEngine.Controllers.Admin
{
    //[Authorize(Roles = "Admin")]
    public class CategoriesAdminController : BaseController
    {
        private readonly DataBaseConnection db;
        private readonly ICategoriesStore categoriesStore;

        public CategoriesAdminController(
            DataBaseConnection db,
            ICategoriesStore categoriesStore,
            UserManager<User> userManager) : base(userManager)
        {
            this.db = db;
            this.categoriesStore = categoriesStore;
        }

        [HttpPost]
        public async Task<IActionResult> GetAllCategories()
        {
            var categories = await db.Categories.OrderBy(x=>x.SortNumber).Select(x => new CategoryAdminViewModel
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

            return Json(root);
        }

        [HttpPost]
        public async Task<IActionResult> CategoryUp(string name)
        {
            using (var transaction = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
            {
                var category = await db.Categories.FirstOrDefaultAsync(x => x.Name == name);
                if (category == null)
                    return BadRequest();

                var category2 = await db.Categories
                    .Where(x => x.ParentId == category.ParentId && x.SortNumber < category.SortNumber)
                    .OrderByDescending(x => x.SortNumber)
                    .FirstOrDefaultAsync();

                if (category2 == null)
                    return BadRequest();

                await db.Categories.Where(x => x.Id == category.Id).Set(x => x.SortNumber, category2.SortNumber)
                    .UpdateAsync();
                await db.Categories.Where(x => x.Id == category2.Id).Set(x => x.SortNumber, category.SortNumber)
                    .UpdateAsync();

                transaction.Complete();
                return Ok();
            }            
        }
        
        [HttpPost]
        public async Task<IActionResult> CategoryDown(string name)
        {
            using (var transaction = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
            {
                var category = await db.Categories.FirstOrDefaultAsync(x => x.Name == name);
                if (category == null)
                    return BadRequest();

                var category2 = await db.Categories
                    .Where(x => x.ParentId == category.ParentId && x.SortNumber > category.SortNumber)
                    .OrderBy(x => x.SortNumber)
                    .FirstOrDefaultAsync();

                if (category2 == null)
                    return BadRequest();

                await db.Categories.Where(x => x.Id == category.Id).Set(x => x.SortNumber, category2.SortNumber)
                    .UpdateAsync();
                await db.Categories.Where(x => x.Id == category2.Id).Set(x => x.SortNumber, category.SortNumber)
                    .UpdateAsync();

                transaction.Complete();
                return Ok();
            }            
        }

        [HttpPost]
        public async Task<IActionResult> ReinitializeCache()
        {
            await categoriesStore.InitializeOrResetAsync();
            return Ok();
        }
    }

    public class CategoryAdminViewModel
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Title { get; set; }

        public bool IsFolder { get; set; }

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