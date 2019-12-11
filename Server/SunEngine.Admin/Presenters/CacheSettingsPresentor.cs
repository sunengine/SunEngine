using System.Linq;
using System.Threading.Tasks;
using LinqToDB;
using SunEngine.Core.DataBase;
using SunEngine.Core.Models;
using SunEngine.Core.Services;

namespace SunEngine.Admin.Presenters
{
    public class CacheSettingsPresentor : DbService
    {
        public CacheSettingsPresentor(DataBaseConnection db) : base(db)
        {
        }

        public async ValueTask<CacheSettings> GetCacheSettings()
        {
            return await db.CacheSettings.OrderBy(x => x.Id).FirstOrDefaultAsync();
        }
    }
}
