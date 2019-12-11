using System.Linq;
using SunEngine.Core.DataBase;
using SunEngine.Core.Models;

namespace SunEngine.Core.Services
{
    public class DbOptionsService
    {
        private DataBaseConnection db;

        public DbOptionsService(DataBaseConnection db) => this.db = db;

        public CacheSettings GetCacheSettings() => db.CacheSettings.OrderBy(x => x.Id).FirstOrDefault();   
    }
}