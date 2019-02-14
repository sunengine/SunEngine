using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using LinqToDB;
using Microsoft.Extensions.Options;
using SunEngine.Configuration.Options;
using SunEngine.DataBase;
using SunEngine.Models;
using SunEngine.Models.Materials;
using SunEngine.Security.Authentication;
using SunEngine.Stores;
using SunEngine.Stores.Models;

namespace SunEngine.Security.Authorization
{
    public class MaterialsAuthorization
    {
        private readonly OperationKeysContainer OperationKeys;
        private readonly IAuthorizationService authorizationService;
        private readonly MaterialOptions materialOptions;
        private readonly ICategoriesStore categoriesStore;
        private readonly DataBaseConnection db;

        public MaterialsAuthorization(IAuthorizationService authorizationService,
            IOptions<MaterialOptions> materialOptions, 
            OperationKeysContainer operationKeysContainer,
            ICategoriesStore categoriesStore,
            DataBaseConnection db)
        {
            this.authorizationService = authorizationService;
            this.materialOptions = materialOptions.Value;
            this.categoriesStore = categoriesStore;
            this.db = db;

            OperationKeys = operationKeysContainer;
        }

        public bool CanAdd(IReadOnlyDictionary<string, RoleStored> userGroups,
            Category category)
        {
            return !category.IsFolder && authorizationService.HasAccess(userGroups, category, OperationKeys.MaterialWrite);
        }

        public bool CanGet(IReadOnlyDictionary<string, RoleStored> userGroups, Category category)
        {
            return authorizationService.HasAccess(userGroups, category, OperationKeys.MaterialAndMessagesRead);
        }


        private bool EditOwnIfTimeNotExceededCheck(DateTime publishDate)
        {
            return DateTime.Now - publishDate <
                   new TimeSpan(0, 0, materialOptions.MaterialsTimeToOwnEditInMinutes, 0, 0);
        }

        private bool DeleteOwnIfTimeNotExceededCheck(DateTime publishDate)
        {
            return DateTime.Now - publishDate <
                   new TimeSpan(0, 0, materialOptions.MaterialsTimeToOwnDeleteInMinutes, 0, 0);
        }

        private bool MoveOwnIfTimeNotExceededCheck(DateTime publishDate)
        {
            return DateTime.Now - publishDate <
                   new TimeSpan(0, 0, materialOptions.MaterialsTimeToOwnDeleteInMinutes, 0, 0);
        }

        public bool CanEdit(MyClaimsPrincipal user, Material material)
        {
            return CanEditAsync(user, material).GetAwaiter().GetResult();
        }

        public async Task<bool> CanEditAsync(MyClaimsPrincipal user, Material material)
        {
            var operationKeys =
                authorizationService.HasAccess(user.UserGroups, material.CategoryId, new[]
                {
                    OperationKeys.MaterialEditOwn,
                    OperationKeys.MaterialEditOwnIfHasReplies,
                    OperationKeys.MaterialEditOwnIfTimeNotExceeded,
                    OperationKeys.MaterialEditAny
                });

            // Если пользователь может редактировать любой материал, то пускаем 
            if (operationKeys.Contains(OperationKeys.MaterialEditAny))
            {
                return true;
            }

            // Если это чужой материал, то запрещаем
            if (material.AuthorId != user.UserId)
            {
                return false;
            }

            // Если MaterialEditOwnIfHasReplies заблокировано и есть чужие ответы то запрещаем
            if (!operationKeys.Contains(OperationKeys.MaterialEditOwnIfHasReplies))
            {
                if (await CheckHasNotOwnRepliesAsync(material, user.UserId))
                {
                    return false;
                }
            }

            // Если MaterialEditOwnIfTimeNotExceeded заблокировано и время редактирования истекло то блокируем
            if (!operationKeys.Contains(OperationKeys.MaterialEditOwnIfTimeNotExceeded))
            {
                if (!EditOwnIfTimeNotExceededCheck(material.PublishDate))
                {
                    return false;
                }
            }

            // Если MaterialEditOwn то разрешаем
            return operationKeys.Contains(OperationKeys.MaterialEditOwn);
        }

        public async Task<bool> CanMoveToTrashAsync(MyClaimsPrincipal user, Material material)
        {
            var operationKeys =
                authorizationService.HasAccess(user.UserGroups, material.CategoryId, new[]
                {
                    OperationKeys.MaterialDeleteAny,
                    OperationKeys.MaterialDeleteOwn,
                    OperationKeys.MaterialDeleteOwnIfTimeNotExceeded,
                    OperationKeys.MaterialDeleteOwnIfHasReplies
                });

            // Если пользователь может редактировать любой материал, то пускаем 
            if (operationKeys.Contains(OperationKeys.MaterialDeleteAny))
            {
                return true;
            }

            // Если это чужой материал, то запрещаем
            if (material.AuthorId != user.UserId)
            {
                return false;
            }

            // Если MaterialEditOwnIfHasReplies заблокировано и есть чужие ответы то запрещаем
            if (!operationKeys.Contains(OperationKeys.MaterialDeleteOwnIfHasReplies))
            {
                if (await CheckHasNotOwnRepliesAsync(material, user.UserId))
                {
                    return false;
                }
            }

            // Если MaterialEditOwnIfTimeNotExceeded заблокировано и время редактирования истекло то блокируем
            if (!operationKeys.Contains(OperationKeys.MaterialDeleteOwnIfTimeNotExceeded))
            {
                if (!EditOwnIfTimeNotExceededCheck(material.PublishDate))
                {
                    return false;
                }
            }

            // Если MaterialEditOwn то разрешаем
            return operationKeys.Contains(OperationKeys.MaterialDeleteOwn);
        }

        // В случае уже имеющегося разрешения на редактирование
        public bool CanMove(MyClaimsPrincipal user, Category categoryFrom, Category categoryTo)
        {
            // Если модератор с правом перемещения материалов на обе категории то разрешаем
            return !categoryTo.IsFolder
                   && authorizationService.HasAccess(user.UserGroups, categoryFrom, OperationKeys.MaterialWrite)
                   && authorizationService.HasAccess(user.UserGroups, categoryTo, OperationKeys.MaterialWrite);
        }

        private bool IsCategoriesFromOneRoot(Category categoryFrom, Category categoryTo)
        {
            return categoriesStore.GetCategoryAreaRoot(categoryFrom).Id ==
                   categoriesStore.GetCategoryAreaRoot(categoryTo).Id;
        }

        private async Task<bool> CheckHasNotOwnRepliesAsync(Material material, int userId)
        {
            return await db.Messages.AnyAsync(x => x.MaterialId == material.Id && x.AuthorId != userId);
        }
    }
}