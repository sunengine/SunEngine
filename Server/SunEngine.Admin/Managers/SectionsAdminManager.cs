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
		Task CreateSectionAsync(Section section);
		Task UpdateSectionAsync(Section section);
		Task DeleteSectionAsync(int sectionId);
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

		public Task CreateSectionAsync(Section section)
		{
			section.Options = section.Options?.MakeJsonTextNotNull();
			section.Roles = rolesCache.CheckAndSetRoles(section.Roles);

			return db.InsertWithIdentityAsync(section);
		}

		public Task UpdateSectionAsync(Section section)
		{
			section.Options = section.Options?.MakeJsonTextNotNull();
			section.Roles = rolesCache.CheckAndSetRoles(section.Roles);

			return db.UpdateAsync(section);
		}

		public Task DeleteSectionAsync(int sectionId)
		{
			return db.Sections.Where(x => x.Id == sectionId).DeleteAsync();
		}
	}
}