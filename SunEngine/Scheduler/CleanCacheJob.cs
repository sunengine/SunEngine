using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using SunEngine.DataBase;
using SunEngine.Security.Authentication;
using SunEngine.Stores;

namespace SunEngine.Scheduler
{
    public class CleanCacheJob : IHostedService
    {
        private readonly SpamProtectionCache spamProtectionCache;
        private readonly JwtBlackListService jwtBlackListService;
        private readonly IDataBaseFactory dbFactory;

        private Timer timerSpamProtectionCache;
        private Timer timerJwtBlackListService;
        private Timer timerLongSessionsClearer;


        public CleanCacheJob(
            IDataBaseFactory dbFactory,
            SpamProtectionCache spamProtectionCache,
            JwtBlackListService jwtBlackListService)
        {
            this.dbFactory = dbFactory;
            this.spamProtectionCache = spamProtectionCache;
            this.jwtBlackListService = jwtBlackListService;
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {

            timerSpamProtectionCache = new Timer(_ =>
            {
                Console.WriteLine("SpamProtectionCache.RemoveExpired");
                spamProtectionCache.RemoveExpired();
            }, null, TimeSpan.Zero, TimeSpan.FromSeconds(10));

            timerJwtBlackListService = new Timer(_ =>
            {
                Console.WriteLine("JwtBlackListService.RemoveExpired");
                jwtBlackListService.RemoveExpired();
            }, null, TimeSpan.Zero, TimeSpan.FromSeconds(12));

            timerLongSessionsClearer = new Timer(_ =>
            {
                Console.WriteLine("LongSessionsClearer.ClearExpiredLongSessions");
                using (var db = dbFactory.CreateDb())
                {
                    LongSessionsClearer.ClearExpiredLongSessions(db);
                }
            }, null, TimeSpan.Zero, TimeSpan.FromSeconds(14));

            return Task.CompletedTask;
        }


        public Task StopAsync(CancellationToken cancellationToken)
        {
            timerSpamProtectionCache.Dispose();
            timerJwtBlackListService.Dispose();
            timerLongSessionsClearer.Dispose();

            return Task.CompletedTask;
        }
    }
}