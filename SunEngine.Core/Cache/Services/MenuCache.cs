using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Threading.Tasks;
using SunEngine.Core.Cache.CacheModels;
using SunEngine.Core.DataBase;
using SunEngine.Core.Models;

namespace SunEngine.Core.Cache.Services
{
    public interface IMenuCache : ISunMemoryCache
    {
        IList<MenuItemCached> GetMenu(IReadOnlyDictionary<string, RoleCached> Roles);
        IReadOnlyList<MenuItemCached> AllMenuItems { get; }
    }

    public class MenuCache : ISunMemoryCache, IMenuCache
    {
        private readonly IDataBaseFactory dataBaseFactory;
        private readonly IRolesCache rolesCache;

        private IReadOnlyList<MenuItemCached> _allMenuItems;

        public IReadOnlyList<MenuItemCached> AllMenuItems
        {
            get
            {
                if (_allMenuItems == null)
                    Initialize();
                return _allMenuItems;
            }
        }

        public MenuCache(
            IDataBaseFactory dataBaseFactory,
            IRolesCache rolesCache)
        {
            this.dataBaseFactory = dataBaseFactory;
            this.rolesCache = rolesCache;
        }


        public IList<MenuItemCached> GetMenu(IReadOnlyDictionary<string, RoleCached> Roles)
        {
            return AllMenuItems.Where(menuItem => Roles.Values.Any(role => menuItem.Roles.ContainsKey(role.Id))).ToList();
        }

        public void Initialize()
        {
            using (var db = dataBaseFactory.CreateDb())
            {
                var menuItems = db.MenuItems.Where(x => !x.IsHidden).ToDictionary(x => x.Id, x => x);
                var allMenuItems = new List<MenuItemCached>(menuItems.Count);

                foreach (var menuItem in menuItems.Values)
                {
                    var roles = menuItem.Roles.Split(',')
                        .Select(x => rolesCache.GetRole(x))
                        .ToDictionary(x => x.Id, x => x)
                        .ToImmutableDictionary();

                    if(CheckIsVisible(menuItem))
                        allMenuItems.Add(new MenuItemCached(menuItem, roles));
                }

                _allMenuItems = allMenuItems.OrderBy(x => x.SortNumber).ToImmutableList();

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
        }

        public Task InitializeAsync()
        {
            throw new System.NotImplementedException();
        }

        public void Reset()
        {
            _allMenuItems = null;
        }
    }
}