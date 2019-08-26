using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SunEngine.Core.Cache.Services;
using SunEngine.Core.Cache.Services.Counters;
using SunEngine.Core.DataBase;
using SunEngine.Core.Errors;
using SunEngine.Core.Filters;
using SunEngine.Core.Managers;
using SunEngine.Core.Models;
using SunEngine.Core.Models.Materials;
using SunEngine.Core.Presenters;
using SunEngine.Core.Security;

namespace SunEngine.Core.Controllers
{
    /// <summary>
    /// Materials CRUD controller.
    /// Used by Articles, Forum and Blog
    /// </summary>
    public class MaterialsController : BaseController
    {
        protected readonly MaterialsAuthorization materialsAuthorization;
        protected readonly ICategoriesCache categoriesCache;
        protected readonly IMaterialsManager materialsManager;
        protected readonly IMaterialsPresenter materialsPresenter;
        protected readonly IMaterialsVisitsCounterCache materialsVisitsCounterCache;
        
        public MaterialsController(
            MaterialsAuthorization materialsAuthorization,
            ICategoriesCache categoriesCache,
            IMaterialsManager materialsManager,
            IMaterialsPresenter materialsPresenter,
            IMaterialsVisitsCounterCache materialsVisitsCounterCache,
            IServiceProvider serviceProvider) : base(serviceProvider)
        {
            this.materialsAuthorization = materialsAuthorization;
            this.categoriesCache = categoriesCache;
            this.materialsManager = materialsManager;
            this.materialsPresenter = materialsPresenter;
            this.materialsVisitsCounterCache = materialsVisitsCounterCache;
        }

        [HttpPost]
        public virtual async Task<IActionResult> Get(string idOrName)
        {
            if (int.TryParse(idOrName, out int id))
                return Get(await materialsPresenter.GetAsync(id));
            else
                return Get(await materialsPresenter.GetAsync(idOrName));
        }

        [NonAction]
        protected virtual IActionResult Get(MaterialView materialView)
        {
            if (materialView == null)
                return BadRequest();

            var category = categoriesCache.GetCategory(materialView.CategoryName);

            if (!materialsAuthorization.CanGet(User.Roles, category))
                return Unauthorized();

            if (materialView.IsHidden && !materialsAuthorization.CanHide(User.Roles, category))
                return Unauthorized();

            if (materialView.DeletedDate != null && !materialsAuthorization.CanRestoreAsync(User, category.Id))
                return Unauthorized();

            materialView.VisitsCount += materialsVisitsCounterCache.CountMaterial(UserOrIpKey, materialView.Id);
            
            return Json(materialView);
        }

        [HttpPost]
        [UserSpamProtectionFilter(TimeoutSeconds = 60)]
        public virtual async Task<IActionResult> Create(MaterialRequestModel materialData)
        {
            var category = categoriesCache.GetCategory(materialData.CategoryName);
            if (category == null)
                return BadRequest();

            if (!materialsAuthorization.CanCreate(User.Roles, category))
                return Unauthorized();

            var now = DateTime.UtcNow;

            var material = new Material
            {
                Title = materialData.Title,
                Text = materialData.text,
                PublishDate = now,
                LastActivity = now,
                CategoryId = category.Id,
                AuthorId = User.UserId
            };

            await SetNameAsync(material, materialData.Name);

            if (category.MaterialsSubTitleInputType == MaterialsSubTitleInputType.Manual)
                material.SubTitle = materialData.SubTitle;

            if (materialData.IsHidden && materialsAuthorization.CanHide(User.Roles, category))
                material.IsHidden = true;

            if (materialData.IsHidden && materialsAuthorization.CanBlockComments(User.Roles, category))
                material.IsCommentsBlocked = true;
            
            if (materialsAuthorization.CanEditSettingsJson(User.Roles, category))
                material.SettingsJson = materialData.SettingsJson;

            contentCache.InvalidateCache(category.Id);

            await materialsManager.CreateAsync(material, materialData.Tags, category);

            return Ok();
        }


        [HttpPost]
        public virtual async Task<IActionResult> Update(MaterialRequestModel materialData)
        {
            if (!ModelState.IsValid)
            {
                var ers = ModelState.Values.SelectMany(v => v.Errors);
                return BadRequest(string.Join(",\n ", ers.Select(x => x.ErrorMessage)));
            }

            Material material = await materialsManager.GetAsync(materialData.Id);
            if (material == null)
                return BadRequest();

            if (!await materialsAuthorization.CanUpdateAsync(User, material))
                return Unauthorized();

            var newCategory = categoriesCache.GetCategory(materialData.CategoryName);
            if (newCategory == null)
                return BadRequest();

            material.Title = materialData.Title;
            material.Text = materialData.text;
            material.EditDate = DateTime.UtcNow;
            
            await SetNameAsync(material, materialData.Name);

            material.SubTitle = newCategory.MaterialsSubTitleInputType == MaterialsSubTitleInputType.Manual
                ? materialData.SubTitle
                : null;

            if (material.IsHidden != materialData.IsHidden && materialsAuthorization.CanHide(User.Roles, newCategory))
                material.IsHidden = materialData.IsHidden;

            if (material.IsCommentsBlocked != materialData.IsCommentsBlocked &&
                materialsAuthorization.CanBlockComments(User.Roles, newCategory))
                material.IsCommentsBlocked = materialData.IsCommentsBlocked;

            if (material.CategoryId != newCategory.Id
                && materialsAuthorization.CanMove(User, categoriesCache.GetCategory(material.CategoryId), newCategory))
                material.CategoryId = newCategory.Id;

            material.SettingsJson = materialsAuthorization.CanEditSettingsJson(User.Roles, newCategory) 
                ? materialData.SettingsJson
                : null;
            
            await materialsManager.UpdateAsync(material, materialData.Tags, newCategory);
            return Ok();
        }

        [NonAction]
        protected async Task SetNameAsync(Material material, string name)
        {
            if (User.IsInRole(RoleNames.Admin))
            {
                if (string.IsNullOrWhiteSpace(name))
                {
                    material.Name = null;
                }
                else
                {
                    if (!materialsManager.IsNameValid(name))
                        throw new SunViewException(new ErrorView("MaterialNameNotValid", "Invalid material name",
                            ErrorType.System));

                    if (name != material.Name && await materialsManager.IsNameInDbAsync(name))
                        throw new SunViewException(ErrorView.SoftError("MaterialNameAlreadyUsed",
                            "This material name is already used"));

                    material.Name = name;
                }
            }
        }

        [HttpPost]
        public virtual async Task<IActionResult> Delete(int id)
        {
            Material material = await materialsManager.GetAsync(id);
            if (material == null)
                return BadRequest();

            if (!await materialsAuthorization.CanDeleteAsync(User, material))
                return Unauthorized();

            contentCache.InvalidateCache(material.CategoryId);

            await materialsManager.DeleteAsync(material);
            return Ok();
        }

        [HttpPost]
        public async Task<IActionResult> Restore(int id)
        {
            Material material = await materialsManager.GetAsync(id);
            if (material == null)
                return BadRequest();

            if (!materialsAuthorization.CanRestoreAsync(User, material.CategoryId))
                return Unauthorized();

            await materialsManager.RestoreAsync(material);

            contentCache.InvalidateCache(material.CategoryId);

            return Ok();
        }

        /// <summary>
        /// Move material up in sort order inside category
        /// </summary>
        [HttpPost]
        public virtual async Task<IActionResult> Up(int id)
        {
            int? categoryId = await materialsManager.GetCategoryIdAsync(id);
            if (!categoryId.HasValue)
                return BadRequest();

            if (materialsAuthorization.CanChangeOrder(User.Roles, categoryId.Value))
                return Unauthorized();

            await materialsManager.UpAsync(id);

            contentCache.InvalidateCache(categoryId.Value);

            return Ok();
        }

        /// <summary>
        /// Move material down in sort order inside category
        /// </summary>
        [HttpPost]
        public virtual async Task<IActionResult> Down(int id)
        {
            int? categoryId = await materialsManager.GetCategoryIdAsync(id);
            if (!categoryId.HasValue)
                return BadRequest();

            if (materialsAuthorization.CanChangeOrder(User.Roles, categoryId.Value))
                return Unauthorized();

            await materialsManager.DownAsync(id);

            contentCache.InvalidateCache(categoryId.Value);

            return Ok();
        }
    }

    public class MaterialRequestModel
    {
        public string Name { get; set; }
        public int Id { get; set; }
        public string CategoryName { get; set; }

        [Required]
        [MinLength(3)]
        [MaxLength(DbColumnSizes.Materials_Title)]
        public string Title { get; set; }

        [MaxLength(DbColumnSizes.Materials_Description)]
        public string SubTitle { get; set; }

        [Required] public string text { get; set; }

        public string Tags { get; set; } = "";
        public DateTime? PublishDate { get; set; } = null;
        public int? AuthorId { get; set; } = null;

        public bool IsHidden { get; set; }
        public bool IsCommentsBlocked { get; set; }
        
        public string SettingsJson { get; set; }
    }
}
