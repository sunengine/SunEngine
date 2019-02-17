using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using SunEngine.Configuration.Options;
using SunEngine.DataBase;
using SunEngine.Models.Materials;
using SunEngine.Security.Authentication;
using SunEngine.Stores.CacheModels;

namespace SunEngine.Security.Authorization
{
    
    public class MessageAuthorization
    {
        private readonly OperationKeysContainer OperationKeys;
        private readonly IAuthorizationService authorizationService;
        private readonly MessagesOptions messagesOptions;
        private readonly DataBaseConnection db;


        public MessageAuthorization(IAuthorizationService authorizationService,
            IOptions<MessagesOptions> messagesOptions,
            OperationKeysContainer operationKeysContainer,
            DataBaseConnection db)
        {
            OperationKeys = operationKeysContainer;

            this.authorizationService = authorizationService;
            this.messagesOptions = messagesOptions.Value;
            this.db = db;
        }

        public bool HasAccessForGetMessages(IReadOnlyDictionary<string,RoleCached> userGroups, int categoryId)
        {
            return authorizationService.HasAccess(userGroups, categoryId, OperationKeys.MaterialAndMessagesRead);
        }

        private bool EditOwnIfTimeNotExceededCheck(DateTime publishDate)
        {
            return DateTime.Now - publishDate < new TimeSpan(0, 0, messagesOptions.TimeToOwnEditInMinutes, 0, 0);
        }

        private bool DeleteOwnIfTimeNotExceededCheck(DateTime publishDate)
        {
            return DateTime.Now - publishDate < new TimeSpan(0, 0, messagesOptions.TimeToOwnDeleteInMinutes, 0, 0);
        }

        public async Task<bool> CanEditAsync(MyClaimsPrincipal user, Message message, int categoryId)
        {
            
            var operationKeys = authorizationService.HasAccess(user.Roles, categoryId, new[]
            {
                OperationKeys.MessageEditOwn, 
                OperationKeys.MessageEditAny,
                OperationKeys.MessageEditOwnIfHasReplies,
                OperationKeys.MessageEditOwnIfTimeNotExceeded
            });

            // Если мы модератор с ключём MessageEditAny разрешаем
            if (operationKeys.Contains(OperationKeys.MessageEditAny))
            {
                return true;
            }

            // Если мы не автор, то блокируем
            if (user.UserId != message.AuthorId)
            {
                return false;
            }

            // Если MessageEditOwnIfHasReplies заблокировано и есть есть чужие ответы далее
            if (operationKeys.Contains(OperationKeys.MessageEditOwnIfHasReplies))
            {
                if (await CheckHasNotOwnAfterAsync(message))
                {
                    return false;
                }
            }
            
            // Если MessageEditOwnIfTimeNotExceeded заблокировано и время редактирования истекло то блокируем
            if (!operationKeys.Contains(OperationKeys.MessageEditOwnIfTimeNotExceeded))
            {
                if (!EditOwnIfTimeNotExceededCheck(message.PublishDate))
                {
                    return false;
                }
            }
            
            // Если MessageEditOwn то разрешаем
            return operationKeys.Contains(OperationKeys.MessageEditOwn);
        }
        
        public async Task<bool> CanMoveToTrashAsync(MyClaimsPrincipal user, Message message, int categoryId)
        {
            var operationKeys = authorizationService.HasAccess(user.Roles, categoryId, new[]
            {
                OperationKeys.MessageDeleteOwn, 
                OperationKeys.MessageDeleteAny,
                OperationKeys.MessageDeleteOwnIfHasReplies,
                OperationKeys.MessageDeleteOwnIfTimeNotExceeded
            });

            // Если мы модератор с ключём MessageDeleteAny разрешаем
            if (operationKeys.Contains(OperationKeys.MessageDeleteAny))
            {
                return true;
            }

            // Если мы не автор, то блокируем
            if (user.UserId != message.AuthorId)
            {
                return false;
            }

            // Если MessageDeleteOwnIfHasReplies заблокировано и есть есть чужие ответы далее
            if (operationKeys.Contains(OperationKeys.MessageDeleteOwnIfHasReplies))
            {
                if (await CheckHasNotOwnAfterAsync(message))
                {
                    return false;
                }
            }
            
            // Если MessageDeleteOwnIfTimeNotExceeded заблокировано и время редактирования истекло то блокируем
            if (!operationKeys.Contains(OperationKeys.MessageDeleteOwnIfTimeNotExceeded))
            {
                if (!DeleteOwnIfTimeNotExceededCheck(message.PublishDate))
                {
                    return false;
                }
            }
            
            // Если MessageDeleteOwn то разрешаем
            return operationKeys.Contains(OperationKeys.MessageDeleteOwn);
        }

        private async Task<bool> CheckHasNotOwnAfterAsync(Message message)
        {
            return await db.Messages.AnyAsync(x => x.MaterialId == message.MaterialId && 
                                                      x.AuthorId != message.AuthorId &&
                                                      x.PublishDate > message.PublishDate);
        }
        
        public bool CanAdd(IReadOnlyDictionary<string,RoleCached> userGroups,int categoryId)
        {
            return authorizationService.HasAccess(userGroups, categoryId,  OperationKeys.MessageWrite);
        } 
    }
}
