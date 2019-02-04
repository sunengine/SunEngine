using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using SunEngine.Authorization;
using SunEngine.Commons.Models;
using SunEngine.Commons.Services;
using SunEngine.EntityServices;
using SunEngine.Filters;
using SunEngine.Stores;

namespace SunEngine.Controllers
{
    public class MaterialsController : BaseController
    {
        private readonly MaterialsAuthorization materialsAuthorization;
        private readonly ICategoriesStore categoriesStore;
        private readonly MaterialsService materialsService;

        public MaterialsController(
            MaterialsAuthorization materialsAuthorization,
            ICategoriesStore categoriesStore, 
            MaterialsService materialsService,
            MyUserManager userManager,
            IUserGroupStore userGroupStore) : base(userGroupStore, userManager)
        {
            this.materialsAuthorization = materialsAuthorization;
            this.categoriesStore = categoriesStore;
            this.materialsService = materialsService;
        }

        [HttpPost]
        public async Task<IActionResult> Get(int id) // TODO Pages
        {
            int? categoryId = await materialsService.GetCategoryIdIfHasMaterialAsync(id);
            if (categoryId == null)
            {
                return BadRequest();
            }

            Category category = categoriesStore.GetCategory(categoryId.Value);

            if (!materialsAuthorization.CanGet(User.UserGroups, category))
            {
                return Unauthorized();
            }


            MaterialViewModel materialViewModel = await materialsService.GetViewModelAsync(id);

            return Json(materialViewModel);
        }


        [HttpPost]
        [UserSpamProtectionFilter(TimeoutSeconds = 60)]
        public async Task<IActionResult> Add(string categoryName,
            string title,
            string text, string tags = "")
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

            await materialsService.InsertAsync(material, tags);

            return Ok();
        }


        [HttpPost]
        public async Task<IActionResult> Edit(int id, string categoryName, string title, string text,
            string tags = "", DateTime? publishDate = null, int? authorId = null)
        {
            Material material = await materialsService.GetAsync(id);
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

            materialsService.UpdateAsync(material, tags);

            return Ok();
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            Material material = await materialsService.GetAsync(id);
            if (material == null)
            {
                return BadRequest();
            }

            if (!await materialsAuthorization.CanMoveToTrashAsync(User, material))
            {
                return Unauthorized();
            }

            await materialsService.MoveToTrashAsync(material);

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