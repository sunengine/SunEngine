using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using LinqToDB;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using SunEngine.Core.Cache.Services;
using SunEngine.Core.Cache.Services.Counters;
using SunEngine.Core.Configuration.Options;
using SunEngine.Core.DataBase;
using SunEngine.Core.Security;

namespace SunEngine.Core.Services
{
    public class CleanCacheJobsService : IHostedService
    {
        private readonly SpamProtectionCache spamProtectionCache;
        private readonly JweBlackListService jweBlackListService;
        private readonly IDataBaseFactory dbFactory;
        private readonly SchedulerOptions schedulerOptions;
        private readonly IMaterialsVisitsCounterCache materialsVisitsCounterCache;
        private readonly IProfilesVisitsCounterService profilesVisitsCounterService;


        private Timer timerSpamProtectionCache;
        private Timer timerJwtBlackListService;
        private Timer timerLongSessionsClearer;
        private Timer timerExpiredRegistrationUsersCleaner;
        private Timer timerCountersUpload;


        public CleanCacheJobsService(
            IDataBaseFactory dbFactory,
            SpamProtectionCache spamProtectionCache,
            IOptions<SchedulerOptions> schedulerOptions,
            IMaterialsVisitsCounterCache materialsVisitsCounterCache,
            IProfilesVisitsCounterService profilesVisitsCounterService,
            JweBlackListService jweBlackListService)
        {
            this.dbFactory = dbFactory;
            this.spamProtectionCache = spamProtectionCache;
            this.jweBlackListService = jweBlackListService;
            this.materialsVisitsCounterCache = materialsVisitsCounterCache;
            this.profilesVisitsCounterService = profilesVisitsCounterService;
            this.schedulerOptions = schedulerOptions.Value;
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            timerSpamProtectionCache = new Timer(_ =>
            {
                Console.WriteLine("SpamProtectionCache.RemoveExpired");
                spamProtectionCache.RemoveExpired();
            }, null, TimeSpan.Zero, TimeSpan.FromMinutes(schedulerOptions.SpamProtectionCacheClearMinutes));

            timerJwtBlackListService = new Timer(_ =>
            {
                Console.WriteLine("JwtBlackListService.RemoveExpired");
                jweBlackListService.RemoveExpired();
            }, null, TimeSpan.Zero, TimeSpan.FromMinutes(schedulerOptions.JwtBlackListServiceClearMinutes));

            timerLongSessionsClearer = new Timer(_ =>
            {
                Console.WriteLine("LongSessionsClearer.ClearExpiredLongSessions");
                using (var db = dbFactory.CreateDb())
                {
                    LongSessionsClearer.ClearExpiredLongSessions(db);
                }
            }, null, TimeSpan.Zero, TimeSpan.FromDays(schedulerOptions.LongSessionsClearDays));

            timerExpiredRegistrationUsersCleaner = new Timer(_ =>
            {
                Console.WriteLine("OldNotRegisteredUsersClearer.CleanOldNotRegisteredUsers");
                using (var db = dbFactory.CreateDb())
                {
                    ExpiredRegistrationUsersClearer.CleanExpiredRegistrationUsers(db);
                }
            }, null, TimeSpan.Zero, TimeSpan.FromDays(schedulerOptions.ExpiredRegistrationUsersClearDays));

            timerCountersUpload = new Timer(_ =>
            {
                Console.WriteLine("CountersUploadToDataBase");
                materialsVisitsCounterCache.UploadToDataBase();
                profilesVisitsCounterService.UploadToDataBase();
            }, null, TimeSpan.FromMinutes(schedulerOptions.UploadVisitsToDataBaseMinutes), TimeSpan.FromMinutes(schedulerOptions.UploadVisitsToDataBaseMinutes)); 

            return Task.CompletedTask;
        }


        public Task StopAsync(CancellationToken cancellationToken)
        {
            timerSpamProtectionCache.Dispose();
            timerJwtBlackListService.Dispose();
            timerLongSessionsClearer.Dispose();
            timerExpiredRegistrationUsersCleaner.Dispose();
            timerCountersUpload.Dispose();
            
            return Task.CompletedTask;
        }
    }

    public static class LongSessionsClearer
    {
        public static void ClearExpiredLongSessions(DataBaseConnection db)
        {
            var now = DateTime.UtcNow;
            db.LongSessions.Where(x => x.ExpirationDate <= now).Delete();
        }
    }

    public static class ExpiredRegistrationUsersClearer
    {
        public static void CleanExpiredRegistrationUsers(DataBaseConnection db)
        {
            DateTime lastLine = DateTime.UtcNow.AddDays(-5);
            db.Users.Where(x => !x.EmailConfirmed && x.RegisteredDate < lastLine).Delete();
        }
    }
}
