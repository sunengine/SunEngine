using System.Linq;
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
		Task CreateComponentAsync(Component component);
		Task UpdateComponentAsync(Component component);
		Task DeleteComponentAsync(int componentId);
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

		public Task CreateComponentAsync(Component component)
		{
			component.ClientSettingsJson = component.ClientSettingsJson?.MakeJsonTextNotNull();
			component.ServerSettingsJson = component.ServerSettingsJson?.MakeJsonTextNotNull();
			component.Roles = rolesCache.CheckAndSetRoles(component.Roles);

			return db.InsertWithIdentityAsync(component);
		}

		public Task UpdateComponentAsync(Component component)
		{
			component.ClientSettingsJson = component.ClientSettingsJson?.MakeJsonTextNotNull();
			component.ServerSettingsJson = component.ServerSettingsJson?.MakeJsonTextNotNull();
			component.Roles = rolesCache.CheckAndSetRoles(component.Roles);

			return db.UpdateAsync(component);
		}

		public Task DeleteComponentAsync(int componentId)
		{
			return db.Components.Where(x => x.Id == componentId).DeleteAsync();
		}
	}
}