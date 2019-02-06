using SunEngine.Commons.DataBase;

namespace SunEngine.Services
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