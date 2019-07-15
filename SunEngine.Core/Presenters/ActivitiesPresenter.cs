using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LinqToDB;
using Microsoft.Extensions.Options;
using SunEngine.Core.Configuration.Options;
using SunEngine.Core.DataBase;
using SunEngine.Core.Services;
using SunEngine.Core.Utils.TextProcess;

namespace SunEngine.Core.Presenters
{
    public interface IActivitiesPresenter
    {
        Task<ActivityView[]> GetActivitiesAsync(
            int[] materialsCategoriesIds, int[] commentsCategoriesIds,
            int number);
    }

    public class ActivitiesPresenter : DbService, IActivitiesPresenter
    {
        protected readonly MaterialsOptions materialsOptions;

        public ActivitiesPresenter(
            IOptions<MaterialsOptions> materialsOptions,
            DataBaseConnection db) : base(db)
        {
            this.materialsOptions = materialsOptions.Value;
        }

        public async Task<ActivityView[]> GetActivitiesAsync(
            int[] materialsCategoriesIds,
            int[] commentsCategoriesIds, int number)
        {
            var materialsActivities = await db.Materials
                .Where(x => materialsCategoriesIds.Contains(x.CategoryId) && !x.IsHidden && x.DeletedDate == null)
                .OrderByDescending(x => x.PublishDate)
                .Take(number)
                .Select(x => new ActivityView
                {
                    MaterialId = x.Id,
                    Title = x.Title,
                    Description = x.SubTitle,
                    CategoryName = x.Category.Name,
                    PublishDate = x.PublishDate,
                    AuthorName = x.Author.UserName,
                    AuthorLink = x.Author.Link,
                    AuthorAvatar = x.Author.Avatar
                }).ToListAsync();

            int descriptionSize = materialsOptions.SubTitleLength;
            int descriptionSizeBig = descriptionSize * 2;

            var commentsActivities = await db.Comments
                .Where(x => commentsCategoriesIds.Contains(x.Material.CategoryId) && x.DeletedDate == null &&
                            !x.Material.IsHidden && x.Material.DeletedDate == null)
                .OrderByDescending(x => x.PublishDate)
                .Take(number)
                .Select(x => new ActivityView
                {
                    MaterialId = x.MaterialId,
                    CommentId = x.Id,
                    Title = x.Material.Title,
                    Description = x.Text.Substring(0, Math.Min(x.Text.Length, descriptionSizeBig)),
                    CategoryName = x.Material.Category.Name,
                    PublishDate = x.PublishDate,
                    AuthorName = x.Author.UserName,
                    AuthorLink = x.Author.Link,
                    AuthorAvatar = x.Author.Avatar
                }).ToListAsync();

            foreach (var comment in commentsActivities)
            {
                comment.Description = SimpleHtmlToText.ClearTagsAndBreaks(comment.Description);
                comment.Description = comment.Description?.Substring(0, Math.Min(comment.Description.Length, descriptionSize));
            }

            List<ActivityView> allActivities = new List<ActivityView>();
            allActivities.AddRange(materialsActivities);
            allActivities.AddRange(commentsActivities);

            return allActivities.OrderByDescending(x => x.PublishDate).Take(number).ToArray();
        }
    }

    public class ActivityView
    {
        public int MaterialId { get; set; }
        public int CommentId { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string CategoryName { get; set; }
        public DateTime PublishDate { get; set; }
        public string AuthorName { get; set; }
        public string AuthorLink { get; set; }
        public string AuthorAvatar { get; set; }
    }
}
