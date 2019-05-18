using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Threading.Tasks;
using SunEngine.Core.Cache.CacheModels;
using SunEngine.Core.DataBase;

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
        private readonly RolesCache rolesCache;

        public IReadOnlyList<MenuItemCached> AllMenuItems { get; private set; }

        public MenuCache(
            IDataBaseFactory dataBaseFactory,
            RolesCache rolesCache)
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
                var menuItems = db.MenuItems.ToDictionary(x => x.Id, x => x);
                var allMenuItems = new List<MenuItemCached>(menuItems.Count);

                foreach (var menuItem in menuItems.Values)
                {
                    var roles = menuItem.Roles.Split(',')
                        .Select(x => rolesCache.GetRole(x))
                        .ToDictionary(x => x.Id, x => x)
                        .ToImmutableDictionary();

                    allMenuItems.Add(new MenuItemCached(menuItem, roles));
                }

                AllMenuItems = allMenuItems.OrderBy(x=>x.SortNumber).ToImmutableList();
            }
        }

        public Task InitializeAsync()
        {
            throw new System.NotImplementedException();
        }

        public void Reset()
        {
            AllMenuItems = null;
        }
    }
}