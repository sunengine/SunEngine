using System;
using System.Linq;
using LinqToDB;
using SunEngine.Commons.DataBase;

namespace SunEngine.Commons.Scheduler
{
    public static class LongSessionsClearer
    {
        public static void ClearExpiredLongSessions(DataBaseConnection db)
        {
            var now = DateTime.UtcNow;
            db.LongSessions.Where(x => x.ExpirationDate <= now).Delete();
        }
    }
}