using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SunEngine.Commons.Filters;
using SunEngine.Commons.Managers;
using SunEngine.Commons.Models.Materials;
using SunEngine.Commons.Presenters;
using SunEngine.Commons.Security;
using IAuthorizationService = SunEngine.Commons.Security.IAuthorizationService;

namespace SunEngine.Commons.Controllers
{
    public class CommentsController : BaseController
    {
        protected readonly OperationKeysContainer OperationKeys;
        protected readonly CommentsAuthorization commentsAuthorization;
        protected readonly IMaterialsManager materialsManager;
        protected readonly ICommentsManager commentsManager;
        protected readonly IAuthorizationService authorizationService;
        protected readonly ICommentsPresenter commentsPresenter;

        public CommentsController(
            IMaterialsManager materialsManager,
            CommentsAuthorization commentsAuthorization,
            OperationKeysContainer operationKeys,
            ICommentsManager commentsManager,
            IAuthorizationService authorizationService,
            ICommentsPresenter commentsPresenter,
            IServiceProvider serviceProvider) : base(serviceProvider)
        {
            OperationKeys = operationKeys;
            this.commentsAuthorization = commentsAuthorization;
            this.materialsManager = materialsManager;
            this.commentsManager = commentsManager;
            this.authorizationService = authorizationService;
            this.commentsPresenter = commentsPresenter;
        }

        [HttpPost]
        public virtual async Task<IActionResult> GetMaterialComments(int materialId)
        {
            int? categoryId = await materialsManager.GetMaterialCategoryIdAsync(materialId);
            if (!categoryId.HasValue)
            {
                return BadRequest();
            }

            if (!commentsAuthorization.HasAccessForGetComments(User.Roles, categoryId.Value))
            {
                return Unauthorized();
            }

            var comments = await commentsPresenter.GetMaterialCommentsAsync(materialId);

            return Json(comments);
        }

        [HttpPost]
        [UserSpamProtectionFilter(TimeoutSeconds = 10)]
        public virtual async Task<IActionResult> Add(int materialId, string text)
        {
            Material material = await materialsManager.GetAsync(materialId);
            if (material == null)
            {
                return BadRequest();
            }

            if (!commentsAuthorization.CanAdd(User.Roles, material.CategoryId))
            {
                return Unauthorized();
            }

            var now = DateTime.UtcNow;
            Comment comment = new Comment
            {
                Material = material,
                MaterialId = materialId,
                PublishDate = now,
                EditDate = now,
                Text = text,
                AuthorId = User.UserId
            };

            await commentsManager.InsertAsync(comment);
            
            contentCache.InvalidateCache(material.CategoryId);


            return Ok();
        }

        [HttpPost]
        public virtual async Task<IActionResult> Get(int id)
        {
            (CommentViewModel commentViewModel, int categoryId) = await commentsPresenter.GetCommentAsync(id);
            if (commentViewModel == null)
            {
                return BadRequest();
            }

            if (!authorizationService.HasAccess(User.Roles, categoryId,
                OperationKeys.MaterialAndCommentsRead))
            {
                return Unauthorized();
            }

            return Json(commentViewModel);
        }

        [HttpPost]
        public virtual async Task<IActionResult> Edit(Comment newComment)
        {
            (Comment comment, int categoryId) = await commentsManager.GetAsync(newComment.Id);
            if (comment == null)
            {
                return BadRequest();
            }

            if (!await commentsAuthorization.CanEditAsync(User, comment, categoryId))
            {
                return Unauthorized();
            }

            comment.Text = newComment.Text;
            comment.EditDate = DateTime.UtcNow;

            await commentsManager.UpdateAsync(comment);

            return Ok();
        }

        [HttpPost]
        public virtual async Task<IActionResult> MoveToTrash(int id)
        {
            (Comment comment, int categoryId) = await commentsManager.GetAsync(id);
            if (comment == null)
            {
                return BadRequest();
            }

            if (!await commentsAuthorization.CanMoveToTrashAsync(User, comment, categoryId))
            {
                return Unauthorized();
            }

            await commentsManager.MoveToTrashAsync(comment);

            contentCache.InvalidateCache(categoryId);
            
            return Ok();
        }

     
    }
}