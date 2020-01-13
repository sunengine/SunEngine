using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using SunEngine.Core.Cache.CacheModels;
using SunEngine.Core.DataBase;
using SunEngine.Core.Models;

namespace SunEngine.Core.Cache.Services
{
	public interface IMenuCache
	{
		IList<MenuItemCached> GetMenu(IReadOnlyDictionary<string, RoleCached> Roles);
		MenuItemCached RootMenuItem { get; }
		IReadOnlyList<MenuItemCached> AllMenuItems { get; }
		void Initialize();
	}

	public class MenuCache : IMenuCache
	{
		private readonly object lockObject = new object();

		protected readonly IDataBaseFactory dataBaseFactory;
		protected readonly IRolesCache rolesCache;

		public MenuItemCached RootMenuItem { get; protected set; }
		public IReadOnlyList<MenuItemCached> AllMenuItems { get; protected set; }


		public MenuCache(
			IDataBaseFactory dataBaseFactory,
			IRolesCache rolesCache)
		{
			this.dataBaseFactory = dataBaseFactory;
			this.rolesCache = rolesCache;
			Initialize();
		}


		public IList<MenuItemCached> GetMenu(IReadOnlyDictionary<string, RoleCached> roles)
		{
			var menuItems = new List<MenuItemCached> {RootMenuItem};

			menuItems.AddRange(AllMenuItems.Where(menuItem =>
				roles.Values.Any(role => menuItem.Roles.ContainsKey(role.Id))));

			return menuItems;
		}

		public void Initialize()
		{
			using var db = dataBaseFactory.CreateDb();

			var menuItems = db.MenuItems.Where(x => !x.IsHidden).ToDictionary(x => x.Id, x => x);
			var allMenuItems = new List<MenuItemCached>(menuItems.Count);

			foreach (var menuItem in menuItems.Values)
			{
				ImmutableDictionary<int, RoleCached> roles;
				if (menuItem.Roles != null)
					roles = menuItem.Roles.Split(',')
						.Select(x => rolesCache.GetRole(x))
						.ToDictionary(x => x.Id, x => x)
						.ToImmutableDictionary();
				else
					roles = new Dictionary<int, RoleCached>().ToImmutableDictionary();


				if (CheckIsVisible(menuItem))
					allMenuItems.Add(new MenuItemCached(menuItem, roles));
			}

			AllMenuItems = allMenuItems.OrderBy(x => x.SortNumber).ToImmutableList();

			RootMenuItem = AllMenuItems.First(x => x.Id == 1);


			bool CheckIsVisible(MenuItem menuItem)
			{
				while (true)
				{
					if (menuItem.IsHidden)
						return false;

					if (!menuItem.ParentId.HasValue)
						return true;

					if (!menuItems.ContainsKey(menuItem.ParentId.Value))
						return false;

					menuItem = menuItems[menuItem.ParentId.Value];
				}
			}
		}
	}
}