using System;
using System.Collections.Concurrent;
using System.Linq;
using System.Threading.Tasks;
using LinqToDB;
using Microsoft.Extensions.Options;
using SunEngine.Core.Cache.Services;
using SunEngine.Core.Configuration.Options;
using SunEngine.Core.DataBase;
using SunEngine.Core.Models.Authorization;

namespace SunEngine.Core.Security
{
    /// <summary>
    /// Store for JWT black listed JWT short tokens
    /// </summary>
    public class JweBlackListService : ISunMemoryCache
    {
        private readonly IDataBaseFactory dataBaseFactory;
        private readonly JweOptions jweOptions;

        private ConcurrentDictionary<string, DateTime> tokens;

        public JweBlackListService(
            IDataBaseFactory dataBaseFactory, 
            IOptions<JweOptions> jweOptions)
        {
            this.dataBaseFactory = dataBaseFactory;
            this.jweOptions = jweOptions.Value;
        }

        private int cycle = 0;

        private void NextTick()
        {
            if (++cycle <= 500) return;

            RemoveExpired();
            cycle = 0;
        }

        public bool IsTokenInBlackList(string shortJwtTokenId)
        {
            if (tokens == null)
                Initialize();

            NextTick();
            return tokens.ContainsKey(shortJwtTokenId);
        }

        public async Task AddAllUserTokensToBlackListAsync(int userId)
        {
            using (var db = dataBaseFactory.CreateDb())
            {
                var sessions = await db.LongSessions.Where(x => x.UserId == userId).ToListAsync();
                DateTime exp = DateTime.UtcNow.AddMinutes(jweOptions.ShortTokenLiveTimeMinutes + 5);

                foreach (var session in sessions)
                    await AddBlackListShortTokenAsync(session.LongToken2, exp);
            }
        }
        
        public async Task AddUserTokensToBlackListAsync(int userId, long[] sessions00)
        {
            using (var db = dataBaseFactory.CreateDb())
            {
                var longSessions = await db.LongSessions.Where(x => x.UserId == userId && sessions00.Contains(x.Id)).ToListAsync();
                DateTime exp = DateTime.UtcNow.AddMinutes(jweOptions.ShortTokenLiveTimeMinutes + 5);

                foreach (var session in longSessions)
                    await AddBlackListShortTokenAsync(session.LongToken2, exp);
            }
        }

        private async Task AddBlackListShortTokenAsync(string long2TokenId, DateTime expired)
        {
            using (var db = dataBaseFactory.CreateDb())
            {
                var token = new BlackListShortToken
                {
                    TokenId = long2TokenId,
                    Expire = expired
                };
                await db.InsertAsync(token);
                tokens.TryAdd(long2TokenId, expired);
            }
        }

        public void Initialize()
        {
            using (var db = dataBaseFactory.CreateDb())
            {
                var tokensDic = db.BlackListShortTokens.ToDictionary(x => x.TokenId, x => x.Expire);
                tokens = new ConcurrentDictionary<string, DateTime>();
                
                foreach (var (key, value) in tokensDic)
                    tokens.TryAdd(key, value);
            }
        }

        public void Reset()
        {
            tokens = null;
        }

        public void RemoveExpired()
        {
            if (tokens == null)
                return;
            
            DateTime now = DateTime.UtcNow;
            int deletedNumber = 0;
            foreach (var (key, value) in tokens.ToArray())
            {
                if (value < now)
                {
                    tokens.TryRemove(key, out _);
                    deletedNumber++;
                }
            }

            if (deletedNumber > 0)
            {
                using (var db = dataBaseFactory.CreateDb())
                {
                    db.BlackListShortTokens.Where(x => x.Expire < now).Delete();
                }
            }
        }
    }
}
