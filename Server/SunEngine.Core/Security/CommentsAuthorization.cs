using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using LinqToDB;
using Microsoft.Extensions.Options;
using SunEngine.Core.Cache.CacheModels;
using SunEngine.Core.Configuration.Options;
using SunEngine.Core.DataBase;
using SunEngine.Core.Models.Materials;

namespace SunEngine.Core.Security
{
	/// <summary>
	/// Authorization helpers for CommentsController
	/// </summary>
	public class CommentsAuthorization
	{
		private readonly OperationKeysContainer OperationKeys;
		private readonly IAuthorizationService authorizationService;
		private readonly IOptionsMonitor<CommentsOptions> commentsOptions;
		private readonly DataBaseConnection db;


		public CommentsAuthorization(
			IAuthorizationService authorizationService,
			IOptionsMonitor<CommentsOptions> commentsOptions,
			OperationKeysContainer operationKeysContainer,
			DataBaseConnection db)
		{
			OperationKeys = operationKeysContainer;

			this.authorizationService = authorizationService;
			this.commentsOptions = commentsOptions;
			this.db = db;
		}

		public bool HasAccessForGetComments(IReadOnlyDictionary<string, RoleCached> userGroups, int categoryId)
		{
			return authorizationService.HasAccess(userGroups, categoryId, OperationKeys.MaterialAndCommentsRead);
		}

		public bool CanSeeDeletedComments(IReadOnlyDictionary<string, RoleCached> userGroups, int categoryId)
		{
			return authorizationService.HasAccess(userGroups, categoryId, OperationKeys.CommentDeleteAny);
		}

		private bool EditOwnIfTimeNotExceededCheck(DateTime publishDate)
		{
			return DateTime.UtcNow - publishDate <
			       new TimeSpan(0, 0, commentsOptions.CurrentValue.TimeToOwnEditInMinutes, 0, 0);
		}

		private bool DeleteOwnIfTimeNotExceededCheck(DateTime publishDate)
		{
			return DateTime.UtcNow - publishDate <
			       new TimeSpan(0, 0, commentsOptions.CurrentValue.TimeToOwnDeleteInMinutes, 0, 0);
		}

		public async Task<bool> CanEditAsync(SunClaimsPrincipal user, Comment comment, int categoryId)
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
					return false;
			}

			// Если CommentEditOwnIfTimeNotExceeded заблокировано и время редактирования истекло то блокируем
			if (!operationKeys.Contains(OperationKeys.CommentEditOwnIfTimeNotExceeded))
			{
				if (!EditOwnIfTimeNotExceededCheck(comment.PublishDate))
					return false;
			}

			// Если CommentEditOwn то разрешаем
			return operationKeys.Contains(OperationKeys.CommentEditOwn);
		}

		public async Task<bool> CanMoveToTrashAsync(SunClaimsPrincipal user, Comment comment, int categoryId)
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

		public bool CanAdd(IReadOnlyDictionary<string, RoleCached> userGroups, Material material)
		{
			if (material.IsCommentsBlocked)
				return false;

			return authorizationService.HasAccess(userGroups, material.CategoryId, OperationKeys.CommentWrite);
		}

		public bool CanAddIfCommentsBlocked(IReadOnlyDictionary<string, RoleCached> userGroups, int categoryId)
		{
			return authorizationService.HasAccess(userGroups, categoryId, OperationKeys.CommentWrite);
		}
	}
}