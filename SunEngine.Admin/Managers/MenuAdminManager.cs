using System;
using System.Linq;
using System.Threading.Tasks;
using LinqToDB;
using SunEngine.Core.DataBase;
using SunEngine.Core.Services;

namespace SunEngine.Admin.Managers
{
    public class MenuAdminManager  : DbService
    {
        public MenuAdminManager(DataBaseConnection db) : base(db)
        {
        }

        public async Task<ServiceResult> MenuItemUp(int id)
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
        
        public async Task<ServiceResult> MenuItemDown(string name)
        {
            var menuItem = await db.MenuItems.FirstOrDefaultAsync(x => x.Name == name);
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

    }
}