using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SunEngine.DataBase;
using SunEngine.Presenters.PagedList;
using SunEngine.Services;

namespace SunEngine.Presenters
{
    public interface IForumPresenter
    {
        Task<IPagedList<TopicInfoViewModel>> GetNewTopics(IList<int> categoryIds, int page,int pageSize,int maxPages);
        Task<IPagedList<TopicInfoViewModel>> GetThread(int categoryId, int page,int pageSize);
    }

    public class ForumPresenter : DbService, IForumPresenter
    {
        public ForumPresenter(DataBaseConnection db) : base(db)
        {
        }

        public virtual Task<IPagedList<TopicInfoViewModel>> GetNewTopics(IList<int> categoryIds, int page,int pageSize,int maxPages)
        {
            
            return db.MaterialsNotDeleted.GetPagedListMaxAsync(
                x => new TopicInfoViewModel
                {
                    Id = x.Id,
                    Title = x.Title,
                    CommentsCount = x.CommentsCount,
                    AuthorName = x.Author.UserName,
                    AuthorAvatar = x.Author.Avatar,
                    PublishDate = x.PublishDate,
                    LastCommentId = x.LastCommentId,
                    LastCommentPublishDate =
                        x.LastCommentId.HasValue ? (DateTime?) x.LastComment.PublishDate : null,
                    CategoryName = x.Category.NameNormalized,
                    CategoryTitle =  x.Category.Title,
                    LastCommentAuthorName = x.LastComment.Author.UserName,
                    LastCommentAuthorAvatar = x.LastComment.Author.Avatar
                },
                x => categoryIds.Contains(x.CategoryId),
                x => x.OrderByDescending(y => y.LastActivity),
                page,
                pageSize,
                maxPages);
        }
        
        public virtual Task<IPagedList<TopicInfoViewModel>> GetThread(int categoryId, int page,int pageSize)
        {
            return db.MaterialsNotDeleted.GetPagedListAsync(
                x => new TopicInfoViewModel
                {
                    Id = x.Id,
                    Title = x.Title,
                    CommentsCount = x.CommentsCount,
                    AuthorName = x.Author.UserName,
                    AuthorAvatar = x.Author.Avatar,
                    PublishDate = x.PublishDate,
                    LastCommentId = x.LastCommentId,
                    LastCommentPublishDate =
                        x.LastCommentId.HasValue ? (DateTime?) x.LastComment.PublishDate : null,
                    CategoryName = x.Category.Name.ToLower(),
                    LastCommentAuthorName = x.LastComment.Author.UserName,
                    LastCommentAuthorAvatar = x.LastComment.Author.Avatar
                },
                x => x.CategoryId == categoryId,
                x => x.OrderByDescending(y => y.LastActivity),
                page,
                pageSize);
        }
    }
    
    /// <summary>
    /// Tопик  в треде (Материал в категории)
    /// </summary>
    public class TopicInfoViewModel
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string AuthorName { get; set; }
        public string AuthorAvatar { get; set; }
        public int CommentsCount { get; set; }
        public DateTime PublishDate { get; set; }
        public int? LastCommentId { get; set; }
        public string LastCommentAuthorName { get; set; }
        public string LastCommentAuthorAvatar { get; set; }
        public DateTime? LastCommentPublishDate { get; set; }
        public string CategoryTitle { get; set; }
        public string CategoryName { get; set; }
    }
}