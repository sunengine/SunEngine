using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SunEngine.Admin.Managers;
using SunEngine.Admin.Presenters;
using SunEngine.Managers;
using SunEngine.Models;
using SunEngine.Stores;

namespace SunEngine.Admin.Controllers
{
    public class AdminCategoriesController : AdminBaseController
    {
        private readonly ICategoriesCache categoriesCache;
        private readonly CategoriesManager categoriesManager;
        private readonly ICategoriesAdminPresenter categoriesAdminPresenter;

        public AdminCategoriesController(
            CategoriesManager categoriesManager,
            ICategoriesAdminPresenter categoriesAdminPresenter,
            ICategoriesCache categoriesCache,
            MyUserManager userManager, IRolesCache rolesCache) : base(rolesCache, userManager)
        {
            this.categoriesCache = categoriesCache;
            this.categoriesManager = categoriesManager;
            this.categoriesAdminPresenter = categoriesAdminPresenter;
        }

        [HttpPost]
        public async Task<IActionResult> GetCategory(int? id = null, string name = null)
        {
            CategoryAdminViewModel category;

            if (id.HasValue)
            {
                category = await categoriesAdminPresenter.GetCategoryAsync(id.Value);
            }
            else if (name != null)
            {
                category = await categoriesAdminPresenter.GetCategoryAsync(name);
            }
            else
            {
                return BadRequest();
            }

            return Json(category);
        }

        [HttpPost]
        public async Task<IActionResult> GetAllCategories()
        {
            var root = await categoriesAdminPresenter.GetAllCategoriesAsync();

            return Json(root);
        }

        [HttpPost]
        public async Task<IActionResult> AddCategory([FromBody] Category category)
        {
            if (!ModelState.IsValid)
            {
                return ValidationProblem();
            }

            await categoriesManager.AddCategoryAsync(category);

            categoriesCache.Reset();

            return Ok();
        }

        [HttpPost]
        public async Task<IActionResult> EditCategory([FromBody] Category category)
        {
            if (!ModelState.IsValid)
            {
                return ValidationProblem();
            }

            await categoriesManager.EditCategoryAsync(category);

            categoriesCache.Reset();

            return Ok();
        }

        [HttpPost]
        public async Task<IActionResult> CategoryUp(string name)
        {
            var rez = await categoriesManager.CategoryUp(name);
            if (rez.Failed)
                return BadRequest();

            categoriesCache.Reset();

            return Ok();
        }

        [HttpPost]
        public async Task<IActionResult> CategoryDown(string name)
        {
            var rez = await categoriesManager.CategoryDown(name);
            if (rez.Failed)
                return BadRequest();

            categoriesCache.Reset();

            return Ok();
        }

        [HttpPost]
        public IActionResult ResetCache()
        {
            categoriesCache.Reset();
            return Ok();
        }
    }
}