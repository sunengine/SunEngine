using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LinqToDB;
using SunEngine.DataBase;
using SunEngine.Models.Authorization;
using SunEngine.Security.Authorization;
using SunEngine.Services;
using SunEngine.Stores;

namespace SunEngine.Security.Authentication
{
    public class JwtBlackListService : IMemoryStore
    {
        private readonly IDataBaseFactory dataBaseFactory;

        private ConcurrentDictionary<string, DateTime> tokens;

        public JwtBlackListService(IDataBaseFactory dataBaseFactory)
        {
            this.dataBaseFactory = dataBaseFactory;
        }

        private int cycle = 0;

        private void NextTick()
        {
            if (++cycle <= 500) return;

            RemoveExpired();
            cycle = 0;
        }

        public bool IsTokenNotInBlackList(string shortJwtTokenId)
        {
            if (tokens == null)
                Initialize();

            NextTick();
            return tokens.ContainsKey(shortJwtTokenId);
        }

        public async Task AddUserTokensAsync(int userId)
        {
            using (var db = dataBaseFactory.CreateDb())
            {
                var sessions = await db.LongSessions.Where(x => x.UserId == userId).ToListAsync();
                DateTime exp = DateTime.UtcNow.AddMinutes(JwtService.ShortTokenLiveTimeMinutes + 5);

                foreach (var session in sessions)
                {
                    Add(session.LongToken2, exp);
                }
            }
        }

        private async Task Add(string long2TokenId, DateTime expired)
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
                {
                    tokens.TryAdd(key, value);
                }
            }
        }

        public async Task InitializeAsync()
        {
            using (var db = dataBaseFactory.CreateDb())
            {
                var tokensDic = await db.BlackListShortTokens.ToDictionaryAsync(x => x.TokenId, x => x.Expire);
                tokens = new ConcurrentDictionary<string, DateTime>();
                foreach (var (key, value) in tokensDic)
                {
                    tokens.TryAdd(key, value);
                }
            }
        }

        public void Reset()
        {
            tokens = null;
        }

        private void RemoveExpired()
        {
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