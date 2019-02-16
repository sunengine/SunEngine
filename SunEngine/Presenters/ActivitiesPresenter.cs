using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LinqToDB;
using Microsoft.Extensions.Options;
using SunEngine.Configuration.Options;
using SunEngine.DataBase;
using SunEngine.Services;
using SunEngine.Utils.TextProcess;

namespace SunEngine.Presenters
{
    public interface IActivitiesPresenter
    {
        Task<ActivityViewModel[]> GetActivitiesAsync(int[] materialsCategoriesIds, int[] messagesCategoriesIds,
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

        public async Task<ActivityViewModel[]> GetActivitiesAsync(int[] materialsCategoriesIds,
            int[] messagesCategoriesIds, int number)
        {
            var materialsActivities = await db.Materials
                .Where(x => materialsCategoriesIds.Contains(x.CategoryId))
                .OrderByDescending(x => x.PublishDate)
                .Take(number)
                .Select(x => new ActivityViewModel
                {
                    MaterialId = x.Id,
                    Title = x.Title,
                    Description = x.Description,
                    CategoryName = x.Category.Name,
                    PublishDate = x.PublishDate,
                    AuthorName = x.Author.UserName,
                    AuthorLink = x.Author.Link,
                    AuthorAvatar = x.Author.Avatar
                }).ToListAsync();

            int descriptionSize = materialsOptions.DescriptionLength;
            int descriptionSizeBig = descriptionSize * 2;

            var messagesActivities = await db.Messages
                .Where(x => messagesCategoriesIds.Contains(x.Material.CategoryId))
                .OrderByDescending(x => x.PublishDate)
                .Take(number)
                .Select(x => new ActivityViewModel
                {
                    MaterialId = x.MaterialId,
                    MessageId = x.Id,
                    Title = x.Material.Title,
                    Description = x.Text.Substring(0, descriptionSizeBig),
                    CategoryName = x.Material.Category.Name,
                    PublishDate = x.PublishDate,
                    AuthorName = x.Author.UserName,
                    AuthorLink = x.Author.Link,
                    AuthorAvatar = x.Author.Avatar
                }).ToListAsync();

            messagesActivities.ForEach(x =>
                x.Description = SimpleHtmlToText.Convert(x.Description).Substring(0, descriptionSize));

            List<ActivityViewModel> allActivities = new List<ActivityViewModel>();
            allActivities.AddRange(materialsActivities);
            allActivities.AddRange(messagesActivities);

            return allActivities.OrderByDescending(x => x.PublishDate).Take(number).ToArray();
        }
    }

    public class ActivityViewModel
    {
        public int MaterialId { get; set; }
        public int MessageId { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string CategoryName { get; set; }
        public DateTime PublishDate { get; set; }
        public string AuthorName { get; set; }
        public string AuthorLink { get; set; }
        public string AuthorAvatar { get; set; }
    }
}