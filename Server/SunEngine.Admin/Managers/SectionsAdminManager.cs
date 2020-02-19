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
	public interface ISectionsAdminManager
	{
		Task CreateComponentAsync(Section section);
		Task UpdateComponentAsync(Section section);
		Task DeleteComponentAsync(int componentId);
	}

	public class SectionsAdminManager : DbService, ISectionsAdminManager
	{
		protected readonly IRolesCache rolesCache;

		public SectionsAdminManager(
			IRolesCache rolesCache,
			DataBaseConnection db) : base(db)
		{
			this.rolesCache = rolesCache;
		}

		public Task CreateComponentAsync(Section section)
		{
			section.Options = section.Options?.MakeJsonTextNotNull();
			section.Roles = rolesCache.CheckAndSetRoles(section.Roles);

			return db.InsertWithIdentityAsync(section);
		}

		public Task UpdateComponentAsync(Section section)
		{
			section.Options = section.Options?.MakeJsonTextNotNull();
			section.Roles = rolesCache.CheckAndSetRoles(section.Roles);

			return db.UpdateAsync(section);
		}

		public Task DeleteComponentAsync(int sectionId)
		{
			return db.Sections.Where(x => x.Id == sectionId).DeleteAsync();
		}
	}
}