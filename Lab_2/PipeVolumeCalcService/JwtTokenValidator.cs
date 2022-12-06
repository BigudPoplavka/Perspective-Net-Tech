using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace PipeVolumeCalcService
{
    public class JwtTokenValidator : ISecurityTokenValidator
    {
        public bool CanReadToken(string securityToken) => true;

        public ClaimsPrincipal ValidateToken(string securityToken, TokenValidationParameters validationParameters, out SecurityToken validatedToken)
        {
            JwtSecurityTokenHandler handler = new JwtSecurityTokenHandler();
            TokenValidationParameters tokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                ValidIssuer = TokenParams.AUTH_CLIENT,
                ValidAudience = TokenParams.AUTH_SERVER,
                IssuerSigningKey = TokenParams.GetSecurityKey()
            };

            ClaimsPrincipal claimsPrincipal = handler.ValidateToken(securityToken, tokenValidationParameters, out validatedToken);
            return claimsPrincipal;
        }

        public bool CanValidateToken { get; } = true;
        public int MaximumTokenSizeInBytes { get; set; } = int.MaxValue;
    }
}
