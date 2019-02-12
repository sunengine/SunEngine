using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SunEngine.Filters;
using SunEngine.Managers;
using SunEngine.Models;
using SunEngine.Models.Materials;
using SunEngine.Presenters;
using SunEngine.Security.Authorization;
using SunEngine.Stores;

namespace SunEngine.Controllers
{
    public class MaterialsController : BaseController
    {
        protected readonly MaterialsAuthorization materialsAuthorization;
        protected readonly ICategoriesStore categoriesStore;
        protected readonly MaterialsManager materialsManager;
        protected readonly IMaterialsPresenter materialsPresenter;

        public MaterialsController(
            MaterialsAuthorization materialsAuthorization,
            ICategoriesStore categoriesStore,
            MaterialsManager materialsManager,
            MyUserManager userManager,
            IMaterialsPresenter materialsPresenter,
            IUserGroupStore userGroupStore) : base(userGroupStore, userManager)
        {
            this.materialsAuthorization = materialsAuthorization;
            this.categoriesStore = categoriesStore;
            this.materialsManager = materialsManager;
            this.materialsPresenter = materialsPresenter;
        }

        [HttpPost]
        public virtual async Task<IActionResult> Get(int id) // TODO Pages
        {
            int? categoryId = await materialsManager.GetCategoryIdIfHasMaterialAsync(id);
            if (categoryId == null)
            {
                return BadRequest();
            }

            Category category = categoriesStore.GetCategory(categoryId.Value);

            if (!materialsAuthorization.CanGet(User.UserGroups, category))
            {
                return Unauthorized();
            }


            var materialViewModel = await materialsPresenter.GetViewModelAsync(id);

            return Json(materialViewModel);
        }


        [HttpPost]
        [UserSpamProtectionFilter(TimeoutSeconds = 60)]
        public virtual async Task<IActionResult> Add(string categoryName, string title, string text, string tags = "")
        {
            Category category = categoriesStore.GetCategory(categoryName);
            if (category == null)
            {
                return BadRequest();
            }

            if (!materialsAuthorization.CanAdd(User.UserGroups, category))
            {
                return Unauthorized();
            }

            var now = DateTime.UtcNow;

            Material material = new Material
            {
                Title = title,
                Text = text,
                PublishDate = now,
                LastActivity = now,
                CategoryId = category.Id,
                AuthorId = User.UserId
            };

            await materialsManager.InsertAsync(material, tags);

            return Ok();
        }


        [HttpPost]
        public virtual async Task<IActionResult> Edit(int id, string categoryName, string title, string text,
            string tags = "", DateTime? publishDate = null, int? authorId = null)
        {
            Material material = await materialsManager.GetAsync(id);
            if (material == null)
            {
                return BadRequest();
            }

            if (!await materialsAuthorization.CanEditAsync(User, material))
            {
                return Unauthorized();
            }

            Category newCategory = categoriesStore.GetCategory(categoryName);
            if (newCategory == null)
            {
                return BadRequest();
            }

            material.Title = title;
            material.Text = text;
            material.EditDate = DateTime.UtcNow;

            // Если категория новая, то обновляем
            if (material.CategoryId != newCategory.Id)
            {
                if (materialsAuthorization.CanMove(User,
                    categoriesStore.GetCategory(material.CategoryId),
                    newCategory))
                {
                    material.CategoryId = newCategory.Id;
                }
            }

            await materialsManager.UpdateAsync(material, tags);

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
}