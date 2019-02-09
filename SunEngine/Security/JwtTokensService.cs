using System.Threading.Tasks;
using LinqToDB;
using Microsoft.AspNetCore.Http;
using SunEngine.DataBase;
using SunEngine.Models;
using SunEngine.Security.Authentication;
using SunEngine.Services;

namespace SunEngine.Security
{
    public class JwtTokensService :  DbService
    {
        public JwtTokensService(DataBaseConnection db) : base(db)
        {
        }
        
        public async Task<MyClaimsPrincipal> RenewSecurityTokensAsync(HttpResponse response, int userId,
            LongSession longSession = null)
        {
            var user = await db.Users.FirstOrDefaultAsync(x => x.Id == userId);
            return await RenewSecurityTokensAsync(response, user, longSession);
        }

        
    }
}