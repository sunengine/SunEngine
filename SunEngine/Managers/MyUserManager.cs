using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LinqToDB;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using SunEngine.DataBase;
using SunEngine.Models;
using SunEngine.Security;
using SunEngine.Utils;

namespace SunEngine.Managers
{
    public class MyUserManager : UserManager<User>
    {
        protected readonly DataBaseConnection db;
        
        public MyUserManager(DataBaseConnection db, IUserStore<User> store, IOptions<IdentityOptions> optionsAccessor, IPasswordHasher<User> passwordHasher, IEnumerable<IUserValidator<User>> userValidators, IEnumerable<IPasswordValidator<User>> passwordValidators, ILookupNormalizer keyNormalizer, IdentityErrorDescriber errors, IServiceProvider services, ILogger<UserManager<User>> logger) : base(store, optionsAccessor, passwordHasher, userValidators, passwordValidators, keyNormalizer, errors, services, logger)
        {
            this.db = db;
            KeyNormalizer = FieldNormalizer.Singleton;
        }    
        
        public virtual Task ChangeEmailAsync(int userId, string email)
        {
            return db.Users.Where(x => x.Id == userId).Set(x => x.Email, email)
                .Set(x => x.NormalizedEmail, FieldNormalizer.Normalize(email))
                .UpdateAsync();
        }

        public virtual Task<bool> CheckEmailInDbAsync(string email, int userId)
        {
            return db.Users.AnyAsync(x => x.NormalizedEmail == FieldNormalizer.Normalize(email) && x.Id != userId);
        }

        public virtual LongSession FindLongSession(LongSession longSession)
        {
            return db.LongSessions.FirstOrDefault(x => x.UserId == longSession.UserId &&
                                                       x.LongToken1 == longSession.LongToken1 &&
                                                       x.LongToken2 == longSession.LongToken2);
        }

        public virtual Task<User> FindByIdAsync(int id)
        {
            return db.Users.FirstOrDefaultAsync(x => x.Id == id);
        }

        public virtual Task<bool> IsUserInRoleAsync(int userId, string roleName)
        {
            var normalizedRoleName = FieldNormalizer.Normalize(roleName);
            return db.UserRoles.AnyAsync(x => x.UserId == userId && x.Role.NormalizedName == normalizedRoleName);
        }

        public override async Task<IdentityResult> AddToRoleAsync(User user, string role)
        {
            if (FieldNormalizer.Normalize(role) == RoleNames.UnregisteredNormalized)
                return IdentityResult.Failed(new IdentityError {Code = "Can_not_add_role_guest", Description = "Can not add guest role"});
            return await base.AddToRoleAsync(user, role);
        }
    }
}