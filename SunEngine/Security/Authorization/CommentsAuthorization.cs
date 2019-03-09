﻿using System;
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
    
    public class CommentsAuthorization
    {
        private readonly OperationKeysContainer OperationKeys;
        private readonly IAuthorizationService authorizationService;
        private readonly CommentsOptions commentsOptions;
        private readonly DataBaseConnection db;


        public CommentsAuthorization(IAuthorizationService authorizationService,
            IOptions<CommentsOptions> commentsOptions,
            OperationKeysContainer operationKeysContainer,
            DataBaseConnection db)
        {
            OperationKeys = operationKeysContainer;

            this.authorizationService = authorizationService;
            this.commentsOptions = commentsOptions.Value;
            this.db = db;
        }

        public bool HasAccessForGetComments(IReadOnlyDictionary<string,RoleCached> userGroups, int categoryId)
        {
            return authorizationService.HasAccess(userGroups, categoryId, OperationKeys.MaterialAndCommentsRead);
        }

        private bool EditOwnIfTimeNotExceededCheck(DateTime publishDate)
        {
            return DateTime.Now - publishDate < new TimeSpan(0, 0, commentsOptions.TimeToOwnEditInMinutes, 0, 0);
        }

        private bool DeleteOwnIfTimeNotExceededCheck(DateTime publishDate)
        {
            return DateTime.Now - publishDate < new TimeSpan(0, 0, commentsOptions.TimeToOwnDeleteInMinutes, 0, 0);
        }

        public async Task<bool> CanEditAsync(MyClaimsPrincipal user, Comment comment, int categoryId)
        {
            
            var operationKeys = authorizationService.HasAccess(user.Roles, categoryId, new[]
            {
                OperationKeys.CommentEditOwn, 
                OperationKeys.CommentEditAny,
                OperationKeys.CommentEditOwnIfHasReplies,
                OperationKeys.CommentEditOwnIfTimeNotExceeded
            });

            // Если мы модератор с ключём CommentEditAny разрешаем
            if (operationKeys.Contains(OperationKeys.CommentEditAny))
            {
                return true;
            }

            // Если мы не автор, то блокируем
            if (user.UserId != comment.AuthorId)
            {
                return false;
            }

            // Если CommentEditOwnIfHasReplies заблокировано и есть есть чужие ответы далее
            if (operationKeys.Contains(OperationKeys.CommentEditOwnIfHasReplies))
            {
                if (await CheckHasNotOwnAfterAsync(comment))
                {
                    return false;
                }
            }
            
            // Если CommentEditOwnIfTimeNotExceeded заблокировано и время редактирования истекло то блокируем
            if (!operationKeys.Contains(OperationKeys.CommentEditOwnIfTimeNotExceeded))
            {
                if (!EditOwnIfTimeNotExceededCheck(comment.PublishDate))
                {
                    return false;
                }
            }
            
            // Если CommentEditOwn то разрешаем
            return operationKeys.Contains(OperationKeys.CommentEditOwn);
        }
        
        public async Task<bool> CanMoveToTrashAsync(MyClaimsPrincipal user, Comment comment, int categoryId)
        {
            var operationKeys = authorizationService.HasAccess(user.Roles, categoryId, new[]
            {
                OperationKeys.CommentDeleteOwn, 
                OperationKeys.CommentDeleteAny,
                OperationKeys.CommentDeleteOwnIfHasReplies,
                OperationKeys.CommentDeleteOwnIfTimeNotExceeded
            });

            // Если мы модератор с ключём CommentDeleteAny разрешаем
            if (operationKeys.Contains(OperationKeys.CommentDeleteAny))
            {
                return true;
            }

            // Если мы не автор, то блокируем
            if (user.UserId != comment.AuthorId)
            {
                return false;
            }

            // Если CommentDeleteOwnIfHasReplies заблокировано и есть есть чужие ответы далее
            if (operationKeys.Contains(OperationKeys.CommentDeleteOwnIfHasReplies))
            {
                if (await CheckHasNotOwnAfterAsync(comment))
                {
                    return false;
                }
            }
            
            // Если CommentDeleteOwnIfTimeNotExceeded заблокировано и время редактирования истекло то блокируем
            if (!operationKeys.Contains(OperationKeys.CommentDeleteOwnIfTimeNotExceeded))
            {
                if (!DeleteOwnIfTimeNotExceededCheck(comment.PublishDate))
                {
                    return false;
                }
            }
            
            // Если CommentDeleteOwn то разрешаем
            return operationKeys.Contains(OperationKeys.CommentDeleteOwn);
        }

        private async Task<bool> CheckHasNotOwnAfterAsync(Comment comment)
        {
            return await db.Comments.AnyAsync(x => x.MaterialId == comment.MaterialId && 
                                                      x.AuthorId != comment.AuthorId &&
                                                      x.PublishDate > comment.PublishDate);
        }
        
        public bool CanAdd(IReadOnlyDictionary<string,RoleCached> userGroups,int categoryId)
        {
            return authorizationService.HasAccess(userGroups, categoryId,  OperationKeys.CommentWrite);
        } 
    }
}