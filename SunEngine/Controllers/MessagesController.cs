using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using SunEngine.Authorization.ControllersAuthorization;
using SunEngine.Commons.Models;
using SunEngine.Commons.Services;
using SunEngine.EntityServices;
using SunEngine.Infrastructure;
using IAuthorizationService = SunEngine.Commons.Services.IAuthorizationService;

namespace SunEngine.Controllers
{
    public class MessagesController : BaseController
    {
        private readonly OperationKeysContainer operationKeysContainer;
        
        private readonly MessageAuthorization messageAuthorization;

        private readonly MaterialsService materialsService;

        private readonly MessagesService messagesService;

        private readonly IAuthorizationService authorizationService;
        

        public MessagesController(
            MaterialsService materialsService, 
            MessageAuthorization messageAuthorization,
            OperationKeysContainer operationKeysContainer, 
            MessagesService messagesService,
            IAuthorizationService authorizationService,
            UserManager<User> userManager) : base(userManager)
        {
            this.operationKeysContainer = operationKeysContainer;
            this.messageAuthorization = messageAuthorization;
            this.materialsService = materialsService;
            this.messagesService = messagesService;
            this.authorizationService = authorizationService;
        }

        [HttpPost]
        public async Task<IActionResult> GetMaterialMessages(int materialId)
        {
            int? categoryId = await materialsService.GetCategoryIdIfHasMaterialAsync(materialId);
            if (!messageAuthorization.HasAccessForGetMessages(User.UserGroups, categoryId.Value))
            {
                return Unauthorized();
            }

            var messages = await messagesService.GetMaterialMessagesAsync(materialId);

            return Json(messages);
        }

        [HttpPost]
        [SpamProtectionFilter(TimeoutSeconds = 10)]
        public async Task<IActionResult> Add(int materialId, string text)
        {
            Material material = await materialsService.GetAsync(materialId);
            if (material == null)
            {
                return BadRequest();
            }

            if (!messageAuthorization.CanAdd(User.UserGroups,material.CategoryId))
            {
                return Unauthorized();
            }

            var now = DateTime.UtcNow;
            Message message = new Message
            {
                Material = material,
                MaterialId = materialId,
                PublishDate = now,
                EditDate = now,
                Text = text,
                AuthorId = User.UserId
            };

            await messagesService.InsertAsync(message);

            return Ok();
        }

        [HttpPost]
        public async Task<IActionResult> Get(int id)
        {
            (MessagesService.MessageViewModel messageViewModel,int categoryId)  = await messagesService.GetViewModelAsync(id);
            if (messageViewModel == null)
            {
                return BadRequest();
            }

            if (!authorizationService.HasAccess(User.UserGroups, categoryId,
                operationKeysContainer.MaterialAndMessagesRead))
            {
                return Unauthorized();
            }

            return Json(messageViewModel);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(Message newMessage)
        {
            (Message message,int categoryId) = await messagesService.GetAsync(newMessage.Id);
            if (message == null)
            {
                return BadRequest();
            }

            if (! await messageAuthorization.CanEditAsync(User, message,categoryId))
            {
                return Unauthorized();
            }

            message.Text = newMessage.Text;
            message.EditDate = DateTime.UtcNow;

            await messagesService.UpdateAsync(message);

            return Ok();
        }

        [HttpPost]
        public async Task<IActionResult> MoveToTrash(int id)
        {
            (Message message,int categoryId) = await messagesService.GetAsync(id);
            if (message == null)
            {
                return BadRequest();
            }

            if (! await messageAuthorization.CanMoveToTrashAsync(User, message, categoryId))
            {
                return Unauthorized();
            }
            
            messagesService.MoveToTrashAsync(message);
            
            return Ok();
        }

        /*[HttpPost]
        public async Task<IActionResult> Restore(int id)
        {
            Message message = await _messagesRepository.Query.Include(x => x.Material).FirstOrDefaultAsync(x => x.Id == id);
            if (message == null)
            {
                return BadRequest();
            }

            if (!_messageAuthorization.CanDelete(User, message, message.Material.CategoryId))
            {
                return Unauthorized();
            }

            _messagesRepository.RestoreFromTrash(message);

            await _messagesRepository.SaveChangesAsync();

            return Ok();
        }*/
    }
}