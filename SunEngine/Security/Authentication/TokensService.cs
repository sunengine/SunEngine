using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using SunEngine.Commons.Models;
using SunEngine.Commons.Utils;
using SunEngine.Options;

namespace SunEngine.Authentication
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