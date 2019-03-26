using SunEngine.Commons.DataBase;

namespace SunEngine.Commons.Services
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