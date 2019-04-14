using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SunEngine.Commons.Cache;
using SunEngine.Commons.DataBase;
using SunEngine.Commons.Filters;
using SunEngine.Commons.Managers;
using SunEngine.Commons.Models.Materials;
using SunEngine.Commons.Presenters;
using SunEngine.Commons.Security;

namespace SunEngine.Commons.Controllers
{
    public class MaterialsController : BaseController
    {
        protected readonly MaterialsAuthorization materialsAuthorization;
        protected readonly ICategoriesCache categoriesCache;
        protected readonly IMaterialsManager materialsManager;
        protected readonly IMaterialsPresenter materialsPresenter;

        public MaterialsController(
            MaterialsAuthorization materialsAuthorization,
            ICategoriesCache categoriesCache,
            IMaterialsManager materialsManager,
            IMaterialsPresenter materialsPresenter,
            IServiceProvider serviceProvider) : base(serviceProvider)
        {
            this.materialsAuthorization = materialsAuthorization;
            this.categoriesCache = categoriesCache;
            this.materialsManager = materialsManager;
            this.materialsPresenter = materialsPresenter;
        }

        [HttpPost]
        public virtual async Task<IActionResult> Get(int id) // TODO Pages
        {
            int? categoryId = await materialsManager.GetMaterialCategoryIdAsync(id);
            if (categoryId == null)
            {
                return BadRequest();
            }

            var category = categoriesCache.GetCategory(categoryId.Value);

            if (!materialsAuthorization.CanGet(User.Roles, category))
            {
                return Unauthorized();
            }
  

            var materialViewModel = await materialsPresenter.GetViewModelAsync(id);

            return Json(materialViewModel);
        }


        [HttpPost]
        [UserSpamProtectionFilter(TimeoutSeconds = 60)]
        public virtual async Task<IActionResult> Add(MaterialRequestModel materialData)
        {
            var category = categoriesCache.GetCategory(materialData.CategoryName);
            if (category == null)
            {
                return BadRequest();
            }

            if (!materialsAuthorization.CanAdd(User.Roles, category))
            {
                return Unauthorized();
            }

            var now = DateTime.UtcNow;

            Material material = new Material
            {
                Title = materialData.Title,
                Text = materialData.text,
                PublishDate = now,
                LastActivity = now,
                CategoryId = category.Id,
                AuthorId = User.UserId
            };

            bool isDescriptionEditable = category.IsDescriptionEditable();
            if (isDescriptionEditable)
            {
                material.Description = materialData.Description;
            }

            contentCache.InvalidateCache(category.Id);
            
            await materialsManager.InsertAsync(material, materialData.Tags, isDescriptionEditable);
            return Ok();
        }


        [HttpPost]
        public virtual async Task<IActionResult> Edit(MaterialRequestModel materialEdited)
        {
            if (!ModelState.IsValid)
            {
                var ers = ModelState.Values.SelectMany(v => v.Errors);
                return BadRequest(string.Join(",\n ",ers.Select(x=>x.ErrorMessage)));
            }
            
            Material materialExisted = await materialsManager.GetAsync(materialEdited.Id);
            if (materialExisted == null)
            {
                return BadRequest();
            }

            if (!await materialsAuthorization.CanEditAsync(User, materialExisted))
            {
                return Unauthorized();
            }

            var newCategory = categoriesCache.GetCategory(materialEdited.CategoryName);
            if (newCategory == null)
            {
                return BadRequest();
            }

            materialExisted.Title = materialEdited.Title;
            materialExisted.Text = materialEdited.text;
            materialExisted.EditDate = DateTime.UtcNow;
            
            bool isDescriptionEditable = newCategory.IsDescriptionEditable();
            materialExisted.Description = isDescriptionEditable ? materialEdited.Description : null;
            
            // Если категория новая, то обновляем
            if (materialExisted.CategoryId != newCategory.Id)
            {
                if (materialsAuthorization.CanMove(User,
                    categoriesCache.GetCategory(materialExisted.CategoryId),
                    newCategory))
                {
                    materialExisted.CategoryId = newCategory.Id;
                }
            }

            await materialsManager.UpdateAsync(materialExisted, materialEdited.Tags, isDescriptionEditable);
            return Ok();
        }

        [HttpPost]
        public virtual async Task<IActionResult> Delete(int id)
        {
            Material material = await materialsManager.GetAsync(id);
            if (material == null)
            {
                return BadRequest();
            }

            if (!await materialsAuthorization.CanMoveToTrashAsync(User, material))
            {
                return Unauthorized();
            }
            
            contentCache.InvalidateCache(material.CategoryId);
            
            await materialsManager.MoveToTrashAsync(material);
            return Ok();
        }


        /*[HttpPost]
        public async Task<IActionResult> Restore(int id)
        {
            Material material = await _materialsRepository.FindAsync(id);
            if (material == null)
            {
                return BadRequest();
            }

            if (!_materialsAuthorization.CanDelete(User, material))
            {
                return Unauthorized();
            }

            await _materialsRepository.RestoreFromTrashAsync(material);

            return Ok();
        }*/
    }

    public class MaterialRequestModel
    {
        public int Id { get; set; }
        public string CategoryName { get; set; }
        [Required]
        [MinLength(3)]
        [MaxLength(DbColumnSizes.Materials_Title)]
        public string Title { get; set; }
        [MaxLength(DbColumnSizes.Materials_Description)]
        public string Description { get; set; }
        [Required]
        public string text { get; set; }

        public string Tags { get; set; } = "";
        public DateTime? PublishDate { get; set; } = null;
        public int? AuthorId { get; set; } = null;
    }
}