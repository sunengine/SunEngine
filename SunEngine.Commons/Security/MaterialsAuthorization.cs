using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using LinqToDB;
using Microsoft.Extensions.Options;
using SunEngine.Commons.Cache.CacheModels;
using SunEngine.Commons.Configuration.Options;
using SunEngine.Commons.DataBase;
using SunEngine.Commons.Models.Materials;

namespace SunEngine.Commons.Security
{
    public class MaterialsAuthorization
    {
        private readonly OperationKeysContainer OperationKeys;
        private readonly IAuthorizationService authorizationService;
        private readonly MaterialsOptions materialsOptions;
        private readonly DataBaseConnection db;

        public MaterialsAuthorization(
            IAuthorizationService authorizationService,
            IOptions<MaterialsOptions> materialsOptions, 
            OperationKeysContainer operationKeysContainer,
            DataBaseConnection db)
        {
            this.authorizationService = authorizationService;
            this.materialsOptions = materialsOptions.Value;
            this.db = db;

            OperationKeys = operationKeysContainer;
        }

        public bool CanAdd(IReadOnlyDictionary<string, RoleCached> userGroups,
            CategoryCached category)
        {
            return category.IsMaterialsContainer && authorizationService.HasAccess(userGroups, category, OperationKeys.MaterialWrite);
        }

        public bool CanGet(IReadOnlyDictionary<string, RoleCached> roles, CategoryCached category)
        {
            return authorizationService.HasAccess(roles, category, OperationKeys.MaterialAndCommentsRead);
        }


        private bool EditOwnIfTimeNotExceededCheck(DateTime publishDate)
        {
            return DateTime.Now - publishDate <
                   new TimeSpan(0, 0, materialsOptions.TimeToOwnEditInMinutes, 0, 0);
        }

        private bool DeleteOwnIfTimeNotExceededCheck(DateTime publishDate)
        {
            return DateTime.Now - publishDate <
                   new TimeSpan(0, 0, materialsOptions.TimeToOwnDeleteInMinutes, 0, 0);
        }

        private bool MoveOwnIfTimeNotExceededCheck(DateTime publishDate)
        {
            return DateTime.Now - publishDate <
                   new TimeSpan(0, 0, materialsOptions.TimeToOwnDeleteInMinutes, 0, 0);
        }

        public bool CanEdit(MyClaimsPrincipal user, Material material)
        {
            return CanEditAsync(user, material).GetAwaiter().GetResult();
        }

        public async Task<bool> CanEditAsync(MyClaimsPrincipal user, Material material)
        {
            var operationKeys =
                authorizationService.HasAccess(user.Roles, material.CategoryId, new[]
                {
                    OperationKeys.MaterialEditOwn,
                    OperationKeys.MaterialEditOwnIfHasReplies,
                    OperationKeys.MaterialEditOwnIfTimeNotExceeded,
                    OperationKeys.MaterialEditAny
                });

            // Если пользователь может редактировать любой материал, то пускаем 
            if (operationKeys.Contains(OperationKeys.MaterialEditAny))
                return true;

            // Если это чужой материал, то запрещаем
            if (material.AuthorId != user.UserId)
                return false;

            // Если MaterialEditOwnIfHasReplies заблокировано и есть чужие ответы то запрещаем
            if (!operationKeys.Contains(OperationKeys.MaterialEditOwnIfHasReplies))
                if (await CheckHasNotOwnRepliesAsync(material, user.UserId))
                    return false;

            // Если MaterialEditOwnIfTimeNotExceeded заблокировано и время редактирования истекло то блокируем
            if (!operationKeys.Contains(OperationKeys.MaterialEditOwnIfTimeNotExceeded))
                if (!EditOwnIfTimeNotExceededCheck(material.PublishDate))
                    return false;

            // Если MaterialEditOwn то разрешаем
            return operationKeys.Contains(OperationKeys.MaterialEditOwn);
        }

        public async Task<bool> CanMoveToTrashAsync(MyClaimsPrincipal user, Material material)
        {
            var operationKeys =
                authorizationService.HasAccess(user.Roles, material.CategoryId, new[]
                {
                    OperationKeys.MaterialDeleteAny,
                    OperationKeys.MaterialDeleteOwn,
                    OperationKeys.MaterialDeleteOwnIfTimeNotExceeded,
                    OperationKeys.MaterialDeleteOwnIfHasReplies
                });

            // Если пользователь может редактировать любой материал, то пускаем 
            if (operationKeys.Contains(OperationKeys.MaterialDeleteAny))
                return true;

            // Если это чужой материал, то запрещаем
            if (material.AuthorId != user.UserId)
                return false;

            // Если MaterialEditOwnIfHasReplies заблокировано и есть чужие ответы то запрещаем
            if (!operationKeys.Contains(OperationKeys.MaterialDeleteOwnIfHasReplies))
                if (await CheckHasNotOwnRepliesAsync(material, user.UserId))
                    return false;

            // Если MaterialEditOwnIfTimeNotExceeded заблокировано и время редактирования истекло то блокируем
            if (!operationKeys.Contains(OperationKeys.MaterialDeleteOwnIfTimeNotExceeded))
                if (!EditOwnIfTimeNotExceededCheck(material.PublishDate))
                    return false;

            // Если MaterialEditOwn то разрешаем
            return operationKeys.Contains(OperationKeys.MaterialDeleteOwn);
        }

        // В случае уже имеющегося разрешения на редактирование
        public bool CanMove(MyClaimsPrincipal user, CategoryCached categoryFrom, CategoryCached categoryTo)
        {
            // Если модератор с правом перемещения материалов на обе категории то разрешаем
            return categoryTo.IsMaterialsContainer
                   && authorizationService.HasAccess(user.Roles, categoryFrom, OperationKeys.MaterialWrite)
                   && authorizationService.HasAccess(user.Roles, categoryTo, OperationKeys.MaterialWrite);
        }

        private async Task<bool> CheckHasNotOwnRepliesAsync(Material material, int userId)
        {
            return await db.Comments.AnyAsync(x => x.MaterialId == material.Id && x.AuthorId != userId);
        }
    }
}