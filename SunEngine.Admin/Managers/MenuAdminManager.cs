using System;
using System.Linq;
using System.Threading.Tasks;
using LinqToDB;
using SunEngine.Core.Cache.Services;
using SunEngine.Core.DataBase;
using SunEngine.Core.Models;
using SunEngine.Core.Security;
using SunEngine.Core.Services;

namespace SunEngine.Admin.Managers
{
    public interface IMenuAdminManager
    {
        Task<ServiceResult> UpAsync(int id);
        Task<ServiceResult> DownAsync(int id);
        Task CreateAsync(MenuItem menuItem);
        Task UpdateAsync(MenuItem menuItem);
        Task<ServiceResult> SetIsHiddenAsync(int id, bool hidden);
        Task DeleteAsync(int menuItemId);
    }

    public class MenuAdminManager : DbService, IMenuAdminManager
    {
        private readonly IRolesCache rolesCache;

        public MenuAdminManager(IRolesCache rolesCache, DataBaseConnection db) : base(db)
        {
            this.rolesCache = rolesCache;
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
            if (menuItem.ParentId == 0)
                menuItem.ParentId = null;
            menuItem.Roles = CheckAndSetRoles(menuItem.Roles);
            return db.InsertWithIdentityAsync(menuItem);
        }

        public virtual Task UpdateAsync(MenuItem menuItem)
        {
            menuItem.Roles = CheckAndSetRoles(menuItem.Roles);
            return db.UpdateAsync(menuItem);
        }

        public virtual async Task<ServiceResult> SetIsHiddenAsync(int id, bool isHidden)
        {
            int lines = await db.MenuItems.Where(x => x.Id == id).Set(x => x.IsHidden, x => isHidden).UpdateAsync();
            return lines == 0 ? ServiceResult.BadResult() : ServiceResult.OkResult();
        }

        public virtual async Task DeleteAsync(int id)
        {
            int lines = await db.MenuItems.Where(x => x.Id == id).DeleteAsync();
            if (lines == 0)
                throw new Exception("No items found to delete");
        }

        protected virtual string CheckAndSetRoles(string roles)
        {
            if (string.IsNullOrWhiteSpace(roles))
                return string.Join(',', RoleNames.Unregistered, RoleNames.Registered);

            var rolesNames = roles.Split(',').Select(x => x.Trim()).ToList().Where(x => rolesCache.AllRoles.ContainsKey(x));
            return string.Join(',', rolesNames);
        }
    }
}
