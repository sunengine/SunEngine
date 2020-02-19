using System.Linq;
using System.Threading.Tasks;
using LinqToDB;
using SunEngine.Core.DataBase;
using SunEngine.Core.Models;
using SunEngine.Core.Services;

namespace SunEngine.Admin.Presenters
{
	public interface ISectionsAdminPresenter
	{
		Task<Section[]> GetSectionsAsync();
		Task<Section> GetComponentAsync(string name);
	}

	public class SectionsAdminPresenter : DbService, ISectionsAdminPresenter
	{
		public SectionsAdminPresenter(DataBaseConnection db) : base(db)
		{
		}

		public Task<Section[]> GetSectionsAsync()
		{
			return db.Sections.OrderBy(x => x.Name).ToArrayAsync();
		}

		public Task<Section> GetComponentAsync(string name)
		{
			return db.Sections.FirstOrDefaultAsync(x => x.Name.ToLower() == name.ToLower());
		}
	}
}