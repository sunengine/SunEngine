using System.Linq;
using System.Threading.Tasks;
using LinqToDB;
using SunEngine.Core.Cache.Services;
using SunEngine.Core.DataBase;
using SunEngine.Core.Errors.Exceptions;
using SunEngine.Core.Models;
using SunEngine.Core.Services;
using SunEngine.Core.Utils;

namespace SunEngine.Admin.Managers
{
	public interface IMenuAdminManager
	{
		Task CreateAsync(MenuItem menuItem);
		Task UpdateAsync(MenuItem menuItem);
		Task UpAsync(int id);
		Task DownAsync(int id);
		Task SetIsHiddenAsync(int id, bool hidden);
		Task DeleteAsync(int menuItemId);
	}

	public class MenuAdminManager : DbService, IMenuAdminManager
	{
		private readonly IRolesCache rolesCache;

		public MenuAdminManager(IRolesCache rolesCache, DataBaseConnection db) : base(db)
		{
			this.rolesCache = rolesCache;
		}

		public virtual async Task CreateAsync(MenuItem menuItem)
		{
			if (menuItem.ParentId == 0)
				throw new SunEntityNotUpdatedException(nameof(MenuItem),
					"Can not create menu item when parentId == null");


			menuItem.Name = menuItem.Name?.SetNullIfEmptyTrim();
			menuItem.Title = menuItem.Title?.SetNullIfEmptyTrim();
			menuItem.SubTitle = menuItem.SubTitle?.SetNullIfEmptyTrim();
			menuItem.RouteName = menuItem.RouteName?.SetNullIfEmptyTrim();
			menuItem.RouteParamsJson = menuItem.RouteParamsJson?.MakeJsonText();
			menuItem.SettingsJson = menuItem.SettingsJson?.MakeJsonText();
			menuItem.CssClass = menuItem.CssClass?.SetNullIfEmptyTrim();
			menuItem.ExternalUrl = menuItem.ExternalUrl?.SetNullIfEmptyTrim();
			menuItem.Icon = menuItem.Icon?.SetNullIfEmptyTrim();
			menuItem.Roles = rolesCache.CheckAndSetRoles(menuItem.Roles);


			using (db.BeginTransaction())
			{
				int id = await db.InsertWithInt32IdentityAsync(menuItem);

				await db.MenuItems.Where(x => x.Id == id).Set(x => x.SortNumber, x => id).UpdateAsync();

				db.CommitTransaction();
			}
		}

		public virtual async Task UpdateAsync(MenuItem menuItem)
		{
			if (menuItem.ParentId == menuItem.Id)
				throw new SunEntityNotUpdatedException(nameof(MenuItem), "Can not set item as self parent");
			if (menuItem.ParentId == null)
				throw new SunEntityNotUpdatedException(nameof(MenuItem),
					"Can not update menu item when parentId == null");

			menuItem.Name = menuItem.Name?.SetNullIfEmptyTrim();
			menuItem.Title = menuItem.Title?.SetNullIfEmptyTrim();
			menuItem.SubTitle = menuItem.SubTitle?.SetNullIfEmptyTrim();
			menuItem.RouteName = menuItem.RouteName?.SetNullIfEmptyTrim();
			menuItem.RouteParamsJson = menuItem.RouteParamsJson?.MakeJsonText();
			menuItem.SettingsJson = menuItem.SettingsJson?.MakeJsonText();
			menuItem.CssClass = menuItem.CssClass?.SetNullIfEmptyTrim();
			menuItem.ExternalUrl = menuItem.ExternalUrl?.SetNullIfEmptyTrim();
			menuItem.Icon = menuItem.Icon?.SetNullIfEmptyTrim();
			menuItem.Roles = rolesCache.CheckAndSetRoles(menuItem.Roles);

			int rowsUpdated = await db.UpdateAsync(menuItem);

			if (rowsUpdated != 1)
				throw new SunEntityNotUpdatedException(nameof(MenuItem), menuItem.Id);
		}

		public virtual async Task UpAsync(int id)
		{
			var menuItem = await db.MenuItems.FirstOrDefaultAsync(x => x.Id == id);
			if (menuItem == null)
				throw new SunEntityNotFoundException(nameof(MenuItem), id);

			var menuItem2 = await db.MenuItems
				.Where(x => x.ParentId == menuItem.ParentId && x.SortNumber < menuItem.SortNumber)
				.OrderByDescending(x => x.SortNumber).FirstOrDefaultAsync();

			if (menuItem2 == null)
				throw new SunEntityNotFoundException(nameof(MenuItem), id);

			using (db.BeginTransaction())
			{
				int rowsUpdated = 0;

				rowsUpdated += await db.MenuItems.Where(x => x.Id == menuItem.Id)
					.Set(x => x.SortNumber, 0)
					.UpdateAsync();

				rowsUpdated += await db.MenuItems.Where(x => x.Id == menuItem2.Id)
					.Set(x => x.SortNumber, menuItem.SortNumber)
					.UpdateAsync();

				rowsUpdated += await db.MenuItems.Where(x => x.Id == menuItem.Id)
					.Set(x => x.SortNumber, menuItem2.SortNumber)
					.UpdateAsync();

				if (rowsUpdated != 3)
					throw new SunEntityNotUpdatedException(nameof(MenuItem), "change position of 2 MenuItems");


				db.CommitTransaction();
			}
		}

		public virtual async Task DownAsync(int id)
		{
			var menuItem = await db.MenuItems.FirstOrDefaultAsync(x => x.Id == id);
			if (menuItem == null)
				throw new SunEntityNotFoundException(nameof(MenuItem), id);

			var menuItem2 = await db.MenuItems
				.Where(x => x.ParentId == menuItem.ParentId && x.SortNumber > menuItem.SortNumber)
				.OrderBy(x => x.SortNumber).FirstOrDefaultAsync();

			if (menuItem2 == null)
				throw new SunEntityNotFoundException(nameof(MenuItem), id);

			using (db.BeginTransaction())
			{
				int rowsUpdated = 0;

				rowsUpdated += await db.MenuItems.Where(x => x.Id == menuItem.Id)
					.Set(x => x.SortNumber, 0)
					.UpdateAsync();

				rowsUpdated += await db.MenuItems.Where(x => x.Id == menuItem2.Id)
					.Set(x => x.SortNumber, menuItem.SortNumber)
					.UpdateAsync();

				rowsUpdated += await db.MenuItems.Where(x => x.Id == menuItem.Id)
					.Set(x => x.SortNumber, menuItem2.SortNumber)
					.UpdateAsync();

				if (rowsUpdated != 3)
					throw new SunEntityNotUpdatedException(nameof(MenuItem), "change position of 2 MenuItems");

				db.CommitTransaction();
			}
		}

		public virtual async Task SetIsHiddenAsync(int id, bool isHidden)
		{
			int rowsUpdated =
				await db.MenuItems.Where(x => x.Id == id).Set(x => x.IsHidden, x => isHidden).UpdateAsync();

			if (rowsUpdated == 0)
				throw new SunEntityNotUpdatedException(nameof(MenuItem), id);
		}

		public virtual async Task DeleteAsync(int id)
		{
			int rowsUpdated = await db.MenuItems.Where(x => x.Id == id).DeleteAsync();

			if (rowsUpdated == 0)
				throw new SunEntityNotDeletedException(nameof(MenuItem), id);
		}
	}
}