using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SunEngine.Admin.Managers;
using SunEngine.Admin.Presenters;
using SunEngine.Core.Cache.Services;
using SunEngine.Core.Models;
using SunEngine.Core.Utils;

namespace SunEngine.Admin.Controllers
{
    public class CategoriesAdminController : BaseAdminController
    {
        private readonly ICategoriesCache categoriesCache;
        private readonly CategoriesAdminManager categoriesAdminManager;
        private readonly ICategoriesAdminPresenter categoriesAdminPresenter;

        public CategoriesAdminController(
            CategoriesAdminManager categoriesAdminManager,
            ICategoriesAdminPresenter categoriesAdminPresenter,
            ICategoriesCache categoriesCache,
            IServiceProvider serviceProvider) : base(serviceProvider)
        {
            this.categoriesCache = categoriesCache;
            this.categoriesAdminManager = categoriesAdminManager;
            this.categoriesAdminPresenter = categoriesAdminPresenter;
        }


        [HttpPost]
        public async ValueTask<IActionResult> GetCategory(int? id = null, string name = null)
        {
            CategoryAdminView category;

            if (id.HasValue)
                category = await categoriesAdminPresenter.GetCategoryAsync(id.Value);
            
            else if (name != null)
                category = await categoriesAdminPresenter.GetCategoryAsync(name);
            
            else
                return BadRequest();
            

            return Json(category);
        }

        [HttpPost]
        public async ValueTask<IActionResult> GetAllCategories()
        {
            var root = await categoriesAdminPresenter.GetAllCategoriesAsync();

            return Json(root);
        }
        
        [HttpPost]
        public IActionResult GetMaterialPreviewGeneratorNames()
        {
            var names = categoriesCache.MaterialsPreviewGenerators.Keys.ToArray();

            return Json(names);
        }

        [HttpPost]
        public async ValueTask<IActionResult> CreateCategory([FromBody] CategoryRequestModel categoryData)
        {
            if (!ModelState.IsValid)
                return ValidationProblem();

            var category = categoryData.ToCategory();

            await categoriesAdminManager.CreateCategoryAsync(category);

            categoriesCache.Reset();
            contentCache.Reset();

            return Ok();
        }

        [HttpPost]
        public async ValueTask<IActionResult> UpdateCategory([FromBody] CategoryRequestModel categoryData)
        {
            if (!ModelState.IsValid)
                return ValidationProblem();
            
            var category = categoryData.ToCategory();

            await categoriesAdminManager.UpdateCategoryAsync(category);

            categoriesCache.Reset();
            contentCache.Reset();

            return Ok();
        }

        [HttpPost]
        public async ValueTask<IActionResult> CategoryUp(string name)
        {
            await categoriesAdminManager.CategoryUp(name);

            categoriesCache.Reset();
            contentCache.Reset();

            return Ok();
        }

        [HttpPost]
        public async ValueTask<IActionResult> CategoryDown(string name)
        {
            await categoriesAdminManager.CategoryDown(name);

            categoriesCache.Reset();
            contentCache.Reset();

            return Ok();
        }

        [HttpPost]
        public async ValueTask<IActionResult> CategoryMoveToTrash(string name)
        {
            await categoriesAdminManager.CategoryMoveToTrashAsync(name);

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

        [Required, MinLength(2), RegularExpression("^[a-zA-Z0-9_-]*$")]
        public string Name { get; set; }

        [Required, MinLength(3)] public string Title { get; set; }
        public string SubTitle { get; set; }
        public string Icon { get; set; }

        public bool IsMaterialsContainer { get; set; }

        public string Header { get; set; }

        public string LayoutName { get; set; }
        
        public bool IsMaterialsNameEditable { get; set; }
        
        public string MaterialsPreviewGeneratorName { get; set; }
        
        public MaterialsSubTitleInputType MaterialsSubTitleInputType { get; set; }
        
        public int ParentId { get; set; }

        public int SortNumber { get; set; }
        
        public string SettingsJson { get; set; }

        public bool IsHidden { get; set; }

        public bool IsCacheContent { get; set; }

        public DateTime? DeletedDate { get; set; }

        public Category ToCategory()
        {
            return new Category
            {
                Id = Id,
                Name = Name,
                Title = Title,
                SubTitle = SubTitle,
                Icon = Icon,
                IsMaterialsContainer = IsMaterialsContainer,
                Header = Header,
                LayoutName = LayoutName,
                IsMaterialsNameEditable = IsMaterialsContainer && IsMaterialsNameEditable,
                MaterialsPreviewGeneratorName = IsMaterialsContainer ?  MaterialsPreviewGeneratorName?.SetNullIfEmpty()  : null,
                MaterialsSubTitleInputType = IsMaterialsContainer ? MaterialsSubTitleInputType : MaterialsSubTitleInputType.None,
                ParentId = ParentId,
                SortNumber = SortNumber,
                DeletedDate = DeletedDate,
                IsHidden = IsHidden,
                IsCacheContent = IsCacheContent,
                SettingsJson = SettingsJson?.MakeJsonText()
            };
        }
    }
}
