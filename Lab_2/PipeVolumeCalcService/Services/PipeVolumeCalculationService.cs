using Grpc.Core;
using Microsoft.AspNetCore.Authorization;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;
using System;
using System.IO;
using System.Collections.Generic;

namespace PipeVolumeCalcService
{
    public class PipeVolumeCalculationService : PipeVolumeCalculation.PipeVolumeCalculationBase
    {
        private readonly ILogger<PipeVolumeCalculationService> _logger;
        private string _usersDB = "usersDB.csv";

        public List<User> authenticatedUsers;

        public PipeVolumeCalculationService(ILogger<PipeVolumeCalculationService> logger)
        {
            _logger = logger;
            authenticatedUsers = new List<User>();
        }


        public override Task<AuthReply> AuthorizeWithSession(AuthRequest authRequest, ServerCallContext context)
        {
            using(StreamReader reader = new StreamReader(_usersDB))
            {
                string DBrow;
                while((DBrow = reader.ReadLine()) != null)
                {
                    var DBcol = DBrow.Split(",");

                    if(DBcol[0] == authRequest.Login && DBcol[1] == authRequest.Password)
                    {
                        authenticatedUsers.Add(new User(authRequest.Login, authRequest.Password));          
                        break;
                    }
                }
            }

            JwtTokenGenerator tokenGenerator = new JwtTokenGenerator(authRequest, 4);

            return Task.FromResult(new AuthReply
            {
                SessionToken = tokenGenerator.GetJwtToken()
            }); 
        }

        public override Task<Reply> Calculate(CalcRequest request, ServerCallContext context)
        {
            var result = request.S * request.L;
            return Task.FromResult(new Reply
            {
                V = result
            });
        }
    }
}










