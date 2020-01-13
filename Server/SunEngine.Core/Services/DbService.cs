using SunEngine.Core.DataBase;

namespace SunEngine.Core.Services
{
	public class DbService
	{
		protected readonly DataBaseConnection db;

		public DbService(DataBaseConnection db)
		{
			this.db = db;
		}
	}
}