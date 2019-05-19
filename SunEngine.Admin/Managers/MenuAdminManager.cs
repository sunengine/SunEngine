using System;
using System.Linq;
using System.Threading.Tasks;
using LinqToDB;
using SunEngine.Core.DataBase;
using SunEngine.Core.Models;
using SunEngine.Core.Services;

namespace SunEngine.Admin.Managers
{
    public interface IMenuAdminManager
    {
        Task<ServiceResult> UpAsync(int id);
        Task<ServiceResult> DownAsync(int id);
        Task CreateAsync(MenuItem menuItem);
        Task UpdateAsync(MenuItem menuItem);
        Task<ServiceResult> SetIsHiddenAsync(int menuItemId, bool hidden);
    }

    public class MenuAdminManager : DbService, IMenuAdminManager
    {
        public MenuAdminManager(DataBaseConnection db) : base(db)
        {
        }

        public virtual async Task<ServiceResult> UpAsync(int id)
        {
            var menuItem = await db.MenuItems.FirstOrDefaultAsync(x => x.Id == id);
            if (menuItem == null)
                return ServiceResult.BadResult();

            var menuItem2 = await db.MenuItems
                .Where(x => x.ParentId == menuItem.ParentId && x.SortNumber < menuItem.SortNumber)
                .OrderByDescending(x => x.SortNumber).FirstOrDefaultAsync();

            if (menuItem2 == null)
                return ServiceResult.BadResult();

            using (db.BeginTransaction())
            {
                await db.MenuItems.Where(x => x.Id == menuItem.Id).Set(x => x.SortNumber, menuItem2.SortNumber)
                    .UpdateAsync();
                await db.MenuItems.Where(x => x.Id == menuItem2.Id).Set(x => x.SortNumber, menuItem.SortNumber)
                    .UpdateAsync();

                db.CommitTransaction();
            }

            return ServiceResult.OkResult();
        }

        public virtual async Task<ServiceResult> DownAsync(int id)
        {
            var menuItem = await db.MenuItems.FirstOrDefaultAsync(x => x.Id == id);
            if (menuItem == null)
                return ServiceResult.BadResult();

            var menuItem2 = await db.MenuItems
                .Where(x => x.ParentId == menuItem.ParentId && x.SortNumber > menuItem.SortNumber)
                .OrderBy(x => x.SortNumber).FirstOrDefaultAsync();

            if (menuItem2 == null)
                return ServiceResult.BadResult();

            using (db.BeginTransaction())
            {
                await db.MenuItems.Where(x => x.Id == menuItem.Id).Set(x => x.SortNumber, menuItem2.SortNumber)
                    .UpdateAsync();
                await db.MenuItems.Where(x => x.Id == menuItem2.Id).Set(x => x.SortNumber, menuItem.SortNumber)
                    .UpdateAsync();

                db.CommitTransaction();
            }

            return ServiceResult.OkResult();
        }

        public virtual Task CreateAsync(MenuItem menuItem)
        {
            menuItem.Roles = CheckAndSetRoles(menuItem.Roles);
            return db.InsertWithIdentityAsync(menuItem);
        }

        public virtual Task UpdateAsync(MenuItem menuItem)
        {
            menuItem.Roles = CheckAndSetRoles(menuItem.Roles);
            return db.UpdateAsync(menuItem);
        }

        public virtual async Task<ServiceResult> SetIsHiddenAsync(int menuItemId, bool isHidden)
        {
            int lines = await db.MenuItems.Where(x => x.Id == menuItemId).Set(x => x.IsHidden, x => isHidden).UpdateAsync();
            return lines == 0 ? ServiceResult.BadResult() : ServiceResult.OkResult();
        }

        protected virtual string CheckAndSetRoles(string Roles)
        {
            return "";
        }
    }
}