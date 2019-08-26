using System;
using System.Collections.Concurrent;
using System.Linq;
using LinqToDB;
using SunEngine.Core.DataBase;
using SunEngine.Core.Filters;

namespace SunEngine.Core.Cache.Services.Counters
{
    public interface IProfilesVisitsCounterService
    {
        int CountProfile(string userOrIpKey, int userId);
        void UploadToDataBase();
    }

    public class ProfilesVisitsCounterService : IProfilesVisitsCounterService
    {
        protected const string SpamProtectionKeyStart = "VISP";

        protected readonly TimeSpan SpamProtectionTimeInterval = TimeSpan.FromMinutes(1);

        protected readonly object lockObject = new object();

        protected readonly ConcurrentDictionary<int, int> visits = new ConcurrentDictionary<int, int>();

        protected readonly IDataBaseFactory dbFactory;
        protected readonly SpamProtectionCache spamProtectionCache;

        public ProfilesVisitsCounterService(
            IDataBaseFactory dbFactory,
            SpamProtectionCache spamProtectionCache)
        {
            this.dbFactory = dbFactory;
            this.spamProtectionCache = spamProtectionCache;
        }


        protected static string GenerateKey(string userOrIpKey, int userId) =>
            SpamProtectionKeyStart + "-" + userOrIpKey + "-" + userId;

        public int CountProfile(string userOrIpKey, int userId)
        {
            var key = GenerateKey(userOrIpKey, userId);

            lock (lockObject)
            {
                if (spamProtectionCache.HasWorkingKey(key))
                {
                    return visits.TryGetValue(userId, out int rez)
                        ? rez
                        : 0;
                }

                spamProtectionCache.AddOrUpdate(key, new RequestFree(SpamProtectionTimeInterval));


                if (visits.TryGetValue(userId, out int materialVisits))
                    return visits[userId] = materialVisits + 1;

                return visits[userId] = 1;
            }
        }

        public void UploadToDataBase()
        {
            UploadIdVisitsToDataBase();
        }

        protected void UploadIdVisitsToDataBase()
        {
            if (visits.Count == 0)
                return;

            lock (lockObject)
                using (var db = dbFactory.CreateDb())
                using (TempTable<VisitsById> visitsByIdTempTable = new TempTable<VisitsById>(db,
                    visits.Select(x => new VisitsById {Id = x.Key, Visits = x.Value}).ToArray()))
                {
                    db.BeginTransaction();

                    db.Users.Where(x => visitsByIdTempTable.Any(y => y.Id == x.Id))
                        .Set(x => x.ProfileVisitsCount,
                            x => x.ProfileVisitsCount + visitsByIdTempTable.FirstOrDefault(y => y.Id == x.Id).Visits)
                        .Update();

                    visits.Clear();
                    db.CommitTransaction();
                }
        }

        protected class VisitsById
        {
            public int Id { get; set; }
            public int Visits { get; set; }
        }
    }
}
