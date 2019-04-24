using System.Linq;
using System.Threading.Tasks;
using LinqToDB;
using SunEngine.Commons.DataBase;
using SunEngine.Commons.Models;

namespace SunEngine.Commons.Services
{
    public class DbOptionsService
    {
        private DataBaseConnection db;

        public DbOptionsService(DataBaseConnection db) => this.db = db;

        public CacheSettings GetCacheSettings() => db.CacheSettings.OrderBy(x => x.Id).FirstOrDefault();   
    }
}