using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SunEngine.Commons.DataBase;
using SunEngine.Commons.PagedList;

namespace SunEngine.EntityServices
{
    public class ForumPresenter : DbService
    {
        public ForumPresenter(DataBaseConnection db) : base(db)
        {
        }

        public Task<IPagedList<TopicInfoViewModel>> GetNewTopics(IList<int> categoryIds, int page,int pageSize,int maxPages)
        {
            
            return db.MaterialsNotDeleted.GetPagedListMaxAsync(
                x => new TopicInfoViewModel
                {
                    Id = x.Id,
                    Title = x.Title,
                    MessagesCount = x.MessagesCount,
                    AuthorName = x.Author.UserName,
                    AuthorAvatar = x.Author.Avatar,
                    PublishDate = x.PublishDate,
                    LastMessageId = x.LastMessageId,
                    LastMessagePublishDate =
                        x.LastMessageId.HasValue ? (DateTime?) x.LastMessage.PublishDate : null,
                    CategoryName = x.Category.Name.ToLower(),
                    CategoryTitle = x.Category.Title,
                    LastMessageAuthorName = x.LastMessage.Author.UserName,
                    LastMessageAuthorAvatar = x.LastMessage.Author.Avatar

                },
                x => categoryIds.Contains(x.CategoryId),
                x => x.OrderByDescending(y => y.LastActivity),
                page,
                pageSize,
                maxPages);
        }
        
        public Task<IPagedList<TopicInfoViewModel>> GetThread(int categoryId, int page,int pageSize)
        {
            return db.MaterialsNotDeleted.GetPagedListAsync(
                x => new TopicInfoViewModel
                {
                    Id = x.Id,
                    Title = x.Title,
                    MessagesCount = x.MessagesCount,
                    AuthorName = x.Author.UserName,
                    AuthorAvatar = x.Author.Avatar,
                    PublishDate = x.PublishDate,
                    LastMessageId = x.LastMessageId,
                    LastMessagePublishDate =
                        x.LastMessageId.HasValue ? (DateTime?) x.LastMessage.PublishDate : null,
                    CategoryName = x.Category.Name.ToLower(),
                    LastMessageAuthorName = x.LastMessage.Author.UserName,
                    LastMessageAuthorAvatar = x.LastMessage.Author.Avatar
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
        public int MessagesCount { get; set; }
        public DateTime PublishDate { get; set; }
        public int? LastMessageId { get; set; }
        public string LastMessageAuthorName { get; set; }
        public string LastMessageAuthorAvatar { get; set; }
        public DateTime? LastMessagePublishDate { get; set; }
        public string CategoryTitle { get; set; }
        public string CategoryName { get; set; }
    }
}