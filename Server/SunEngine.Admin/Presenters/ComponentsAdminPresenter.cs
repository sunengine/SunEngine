using System.Linq;
using System.Threading.Tasks;
using LinqToDB;
using SunEngine.Core.DataBase;
using SunEngine.Core.Models;
using SunEngine.Core.Services;

namespace SunEngine.Admin.Presenters
{
	public interface IComponentsAdminPresenter
	{
		Task<Component[]> GetComponentsAsync();
		Task<Component> GetComponentAsync(string name);
	}

	public class ComponentsAdminPresenter : DbService, IComponentsAdminPresenter
	{
		public ComponentsAdminPresenter(DataBaseConnection db) : base(db)
		{
		}

		public Task<Component[]> GetComponentsAsync()
		{
			return db.Components.OrderBy(x => x.Name).ToArrayAsync();
		}

		public Task<Component> GetComponentAsync(string name)
		{
			return db.Components.FirstOrDefaultAsync(x => x.Name.ToLower() == name.ToLower());
		}
	}
}