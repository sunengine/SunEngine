using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LinqToDB;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using SunEngine.Commons.DataBase;
using SunEngine.Commons.Models;

namespace SunEngine.Commons.Services
{
    public class MyUserManager : UserManager<User>
    {
        private readonly DataBaseConnection db;
        
        public MyUserManager(DataBaseConnection db, IUserStore<User> store, IOptions<IdentityOptions> optionsAccessor, IPasswordHasher<User> passwordHasher, IEnumerable<IUserValidator<User>> userValidators, IEnumerable<IPasswordValidator<User>> passwordValidators, ILookupNormalizer keyNormalizer, IdentityErrorDescriber errors, IServiceProvider services, ILogger<UserManager<User>> logger) : base(store, optionsAccessor, passwordHasher, userValidators, passwordValidators, keyNormalizer, errors, services, logger)
        {
            this.db = db;
            KeyNormalizer = Normalizer.Singleton;
        }    
        
        public Task ChangeEmailAsync(int userId, string email)
        {
            return db.Users.Where(x => x.Id == userId).Set(x => x.Email, email)
                .Set(x => x.NormalizedEmail, Normalizer.Singleton.Normalize(email))
                .UpdateAsync();
        }

        public Task<bool> CheckEmailInDbAsync(string email, int userId)
        {
            return db.Users.AnyAsync(x => x.NormalizedEmail == email.ToUpper() && x.Id != userId);
        }

        public LongSession FindLongSession(LongSession longSession)
        {
            return db.LongSessions.FirstOrDefault(x => x.UserId == longSession.UserId &&
                                                       x.LongToken1 == longSession.LongToken1 &&
                                                       x.LongToken2 == longSession.LongToken2);
        }
    }
}