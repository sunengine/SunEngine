using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SunEngine.DataBase;
using SunEngine.Services;

namespace SunEngine.Presenters
{
    public interface IActivitiesPresenter
    {
        Task<ActivityViewModel[]> GetActivitiesAsync(int[] materialsCategoriesIds, int[] messagesCategoriesIds, int number);
    }

    public class ActivitiesPresenter : DbService, IActivitiesPresenter
    {
        public ActivitiesPresenter(DataBaseConnection db) : base(db)
        {
        }

        public async Task<ActivityViewModel[]> GetActivitiesAsync(int[] materialsCategoriesIds,
            int[] messagesCategoriesIds, int number)
        {
            var materialsActivities = db.Materials
                .Where(x => materialsCategoriesIds.Contains(x.Id))
                .OrderByDescending(x => x.PublishDate)
                .Take(number)
                .Select(x => new ActivityViewModel
                {
                    IsMaterial = true,
                    Id = x.Id,
                    Title = x.Title,
                    Description = x.Description,
                    CategoryName = x.Category.Name,
                    PublishDate = x.PublishDate
                });

            var messagesActivities = db.Messages
                .Where(x => messagesCategoriesIds.Contains(x.Id))
                .OrderByDescending(x => x.PublishDate)
                .Take(number)
                .Select(x => new ActivityViewModel
                {
                    IsMaterial = false,
                    Id = x.Id,
                    Title = x.Material.Title,
                    Description = x.Text,
                    CategoryName = x.Material.Category.Name,
                    PublishDate = x.PublishDate
                });

            List<ActivityViewModel> allActivities = new List<ActivityViewModel>();
            allActivities.AddRange(materialsActivities);
            allActivities.AddRange(messagesActivities);

            return allActivities.OrderByDescending(x => x.PublishDate).Take(number).ToArray();
        }
    }

    public class ActivityViewModel
    {
        public bool IsMaterial { get; set; }
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string CategoryName { get; set; }
        public DateTime PublishDate { get; set; }
    }
}