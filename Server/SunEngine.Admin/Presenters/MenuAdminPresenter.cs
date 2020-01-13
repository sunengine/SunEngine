using System.Linq;
using System.Threading.Tasks;
using LinqToDB;
using SunEngine.Core.DataBase;
using SunEngine.Core.Models;
using SunEngine.Core.Services;

namespace SunEngine.Admin.Presenters
{
	public interface IMenuAdminPresenter
	{
		Task<MenuItem[]> GetMenuItemsAsync();
		Task<MenuItem> GetMenuItemAsync(int id);
	}

	public class MenuAdminPresenter : DbService, IMenuAdminPresenter
	{
		public MenuAdminPresenter(DataBaseConnection db) : base(db)
		{
		}

		public Task<MenuItem[]> GetMenuItemsAsync()
		{
			return db.MenuItems.OrderBy(x => x.SortNumber).ToArrayAsync();
		}

		public Task<MenuItem> GetMenuItemAsync(int id)
		{
			return db.MenuItems.FirstOrDefaultAsync(x => x.Id == id);
		}
	}
}