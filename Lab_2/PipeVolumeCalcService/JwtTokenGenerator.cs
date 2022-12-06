using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace PipeVolumeCalcService
{
    public struct TokenParams
    {
        public const string AUTH_SERVER = "Auth_Server"; 
        public const string AUTH_CLIENT = "Auth_Client"; 
        const string KEY = "#1auth_key1#";   
        public static SymmetricSecurityKey GetSecurityKey()
        {
            return new SymmetricSecurityKey(Encoding.UTF8.GetBytes(KEY));
        }
    }

    public class JwtTokenGenerator
    {
        private AuthRequest _authRequest;
        private int _expireTime;

        public JwtTokenGenerator(AuthRequest authRequest, int expireTime)
        {
            _authRequest = authRequest;
            _expireTime = expireTime;
        }

        public string GetJwtToken()
        {
            List<Claim> claims = new List<Claim> { new Claim(ClaimTypes.Name, _authRequest.Login) };
            JwtSecurityToken jwtToken = new JwtSecurityToken(
                    issuer: TokenParams.AUTH_SERVER,
                    audience: TokenParams.AUTH_CLIENT,
                    claims: claims,
                    expires: DateTime.UtcNow.Add(TimeSpan.FromMinutes(_expireTime)), 
                    signingCredentials: new SigningCredentials(TokenParams.GetSecurityKey(), SecurityAlgorithms.HmacSha256));

            JwtSecurityTokenHandler handler = new JwtSecurityTokenHandler();
            return jwtToken.EncodedPayload;
        }
    }
}
