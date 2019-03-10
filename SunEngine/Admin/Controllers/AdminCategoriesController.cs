using System;
using System.ComponentModel.DataAnnotations;
using System.Threading;
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
            IServiceProvider serviceProvider) : base(serviceProvider)
        {
            this.categoriesCache = categoriesCache;
            this.categoriesManager = categoriesManager;
            this.categoriesAdminPresenter = categoriesAdminPresenter;
        }

        [HttpPost]
        public async Task<IActionResult> GetAllSectionTypes()
        {
            var sectionTypes = await categoriesAdminPresenter.GetAllSectionTypesAsync();

            return Json(sectionTypes);
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
        public async Task<IActionResult> AddCategory([FromBody] CategoryRequestModel categoryData)
        {
            if (!ModelState.IsValid)
            {
                return ValidationProblem();
            }

            var sectionType = categoriesCache.AllSectionTypes.ContainsKey(categoryData.SectionTypeName)
                ? categoriesCache.AllSectionTypes[categoryData.SectionTypeName]
                : null;

            var category = categoryData.ToCategory();
            category.SectionTypeId = sectionType?.Id;

            if (!categoryData.AppendUrlToken.HasValue)
            {
                if (sectionType != null)
                    category.AppendUrlToken = true;                
            }
            else
            {
                category.AppendUrlToken = categoryData.AppendUrlToken.Value;
            }

            await categoriesManager.AddCategoryAsync(category);

            categoriesCache.Reset();
            contentCache.Reset();

            return Ok();
        }

        [HttpPost]
        public async Task<IActionResult> EditCategory([FromBody] CategoryRequestModel categoryData)
        {
            if (!ModelState.IsValid)
            {
                return ValidationProblem();
            }

            var sectionType = categoriesCache.AllSectionTypes.ContainsKey(categoryData.SectionTypeName)
                ? categoriesCache.AllSectionTypes[categoryData.SectionTypeName]
                : null;

            var category = categoryData.ToCategory();
            category.SectionTypeId = sectionType?.Id;

            if (!categoryData.AppendUrlToken.HasValue)
            {
                if (sectionType != null)
                    category.AppendUrlToken = true;
            }
            else
            {
                category.AppendUrlToken = categoryData.AppendUrlToken.Value;
            }

            await categoriesManager.EditCategoryAsync(category);

            categoriesCache.Reset();
            contentCache.Reset();

            return Ok();
        }

        [HttpPost]
        public async Task<IActionResult> CategoryUp(string name)
        {
            var rez = await categoriesManager.CategoryUp(name);
            if (rez.Failed)
                return BadRequest();

            categoriesCache.Reset();
            contentCache.Reset();

            return Ok();
        }

        [HttpPost]
        public async Task<IActionResult> CategoryDown(string name)
        {
            var rez = await categoriesManager.CategoryDown(name);
            if (rez.Failed)
                return BadRequest();

            categoriesCache.Reset();
            contentCache.Reset();

            return Ok();
        }
        
        [HttpPost]
        public async Task<IActionResult> CategoryMoveToTrash(string name)
        {
            await categoriesManager.CategoryMoveToTrashAsync(name);

            categoriesCache.Reset();
            contentCache.Reset();

            return Ok();
        }

        [HttpPost]
        public IActionResult ResetCache()
        {
            categoriesCache.Reset();
            contentCache.Reset();
            return Ok();
        }
    }

    public class CategoryRequestModel
    {
        public int Id { get; set; }

        [Required, MinLength(2), RegularExpression("^[a-zA-Z-]*$")]
        public string Name { get; set; }

        [Required, MinLength(3)] public string Title { get; set; }
        public bool IsMaterialsContainer { get; set; }

        public string Description { get; set; }

        public string Header { get; set; }

        public bool? AppendUrlToken { get; set; }

        public string SectionTypeName { get; set; }

        public int ParentId { get; set; }

        public int SortNumber { get; set; }

        public bool IsDeleted { get; set; }

        public bool IsHidden { get; set; }
        
        public bool IsCacheContent { get; set; }

        public Category ToCategory()
        {
            return new Category
            {
                Id = Id,
                Name = Name,
                Title = Title,
                IsMaterialsContainer = IsMaterialsContainer,
                Description = Description,
                Header = Header,
                //AppendUrlToken = AppendUrlToken,
                ParentId = ParentId,
                SortNumber = SortNumber,
                IsDeleted = IsDeleted,
                IsHidden = IsHidden,
                IsCacheContent = IsCacheContent
            };
        }
    }
}