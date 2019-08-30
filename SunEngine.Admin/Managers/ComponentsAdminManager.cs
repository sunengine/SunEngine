using System.Threading.Tasks;
using LinqToDB;
using SunEngine.Core.Cache.Services;
using SunEngine.Core.DataBase;
using SunEngine.Core.Models;
using SunEngine.Core.Services;
using SunEngine.Core.Utils;

namespace SunEngine.Admin.Managers
{
    public interface IComponentsAdminManager
    {
        Task CreateComponent(Component component);
    }

    public class ComponentsAdminManager : DbService, IComponentsAdminManager
    {
        protected readonly IRolesCache rolesCache;
        
        public ComponentsAdminManager(
            IRolesCache rolesCache,
            DataBaseConnection db) : base(db)
        {
            this.rolesCache = rolesCache;
        }
        
        public Task CreateComponent(Component component)
        {
            component.ClientSettingsJson = component.ClientSettingsJson?.MakeJsonTextNotNull();
            component.ServerSettingsJson = component.ServerSettingsJson?.MakeJsonTextNotNull();
            component.Roles = rolesCache.CheckAndSetRoles(component.Roles);
            
            return db.InsertWithIdentityAsync(component);
        }
    }
}
