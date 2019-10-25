using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using SunEngine.Core.Cache.CacheModels;
using SunEngine.Core.DataBase;
using SunEngine.Core.Models;

namespace SunEngine.Core.Cache.Services
{
    public interface IMenuCache : ISunMemoryCache
    {
        IList<MenuItemCached> GetMenu(IReadOnlyDictionary<string, RoleCached> Roles);
    }

    public class MenuCache : IMenuCache
    {
        private readonly object lockObject = new object();

        private readonly IDataBaseFactory dataBaseFactory;
        private readonly IRolesCache rolesCache;

        private MenuItemCached _rootMenuItem;
        private IReadOnlyList<MenuItemCached> _allMenuItems;

        #region Getters

        protected MenuItemCached RootMenuItem
        {
            get
            {
                lock (lockObject)
                    if (_rootMenuItem == null)
                        Initialize();

                return _rootMenuItem;
            }
        }

        protected IReadOnlyList<MenuItemCached> AllMenuItems
        {
            get
            {
                lock (lockObject)
                    if (_allMenuItems == null)
                        Initialize();

                return _allMenuItems;
            }
        }

        #endregion

        public MenuCache(
            IDataBaseFactory dataBaseFactory,
            IRolesCache rolesCache)
        {
            this.dataBaseFactory = dataBaseFactory;
            this.rolesCache = rolesCache;
        }


        public IList<MenuItemCached> GetMenu(IReadOnlyDictionary<string, RoleCached> Roles)
        {
            var menuItems = new List<MenuItemCached> {RootMenuItem};

            menuItems.AddRange(AllMenuItems.Where(menuItem =>
                Roles.Values.Any(role => menuItem.Roles.ContainsKey(role.Id))));

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
                {
                    roles = menuItem.Roles.Split(',')
                        .Select(x => rolesCache.GetRole(x))
                        .ToDictionary(x => x.Id, x => x)
                        .ToImmutableDictionary();
                }
                else
                {
                    roles = new Dictionary<int, RoleCached>().ToImmutableDictionary();
                }


                if (CheckIsVisible(menuItem))
                    allMenuItems.Add(new MenuItemCached(menuItem, roles));
            }

            _allMenuItems = allMenuItems.OrderBy(x => x.SortNumber).ToImmutableList();

            _rootMenuItem = _allMenuItems.First(x => x.Id == 1);


            bool CheckIsVisible(MenuItem menuItem)
            {
                if (menuItem.IsHidden)
                    return false;

                if (menuItem.ParentId.HasValue)
                {
                    if (!menuItems.ContainsKey(menuItem.ParentId.Value))
                        return false;

                    return CheckIsVisible(menuItems[menuItem.ParentId.Value]);
                }

                return true;
            }
        }

        public void Reset()
        {
            _allMenuItems = null;
        }
    }
}