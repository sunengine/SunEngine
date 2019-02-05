using Microsoft.Extensions.Options;
using SunEngine.Configuration.Options;

namespace SunEngine.Security.Authentication
{
    public class TokensService
    {
        private readonly JwtOptions jwtOptions;
        
        public TokensService(IOptions<JwtOptions> jwtOptions)
        {
            this.jwtOptions = jwtOptions.Value;
        }
        
        
    }
}