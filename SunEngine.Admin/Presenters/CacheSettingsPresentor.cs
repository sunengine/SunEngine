using System.Linq;
using System.Threading.Tasks;
using LinqToDB;
using SunEngine.Commons.DataBase;
using SunEngine.Commons.Models;
using SunEngine.Commons.Services;

namespace SunEngine.Admin.Presenters
{
    public class CacheSettingsPresentor : DbService
    {
        public CacheSettingsPresentor(DataBaseConnection db) : base(db)
        {
        }

        public async Task<CacheSettings> GetCacheSettings()
        {
            return await db.CacheSettings.OrderBy(x => x.Id).FirstOrDefaultAsync();
        }
    }
}