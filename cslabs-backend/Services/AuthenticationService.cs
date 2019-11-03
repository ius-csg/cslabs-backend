using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using CSLabsBackend.Config;
using CSLabsBackend.Models;
using CSLabsBackend.Util;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace CSLabsBackend.Services
{
    public interface IAuthenticationService
    {
        User Authenticate(string email, string password);
    }

    public class AuthenticationService : IAuthenticationService
    {
        private readonly AppSettings _appSettings;
        private readonly DefaultContext _databaseContext;
        
        public AuthenticationService(IOptions<AppSettings> appSettings, DefaultContext defaultContext)
        {
            _appSettings = appSettings.Value;
            _databaseContext = defaultContext;
        }

        public User Authenticate(string email, string password)
        {
            // @todo authenticate with kerberos.
            var user = _databaseContext.Users.SingleOrDefault(x => x.SchoolEmail == email || x.PersonalEmail == email);


            // return null if user not found
            if (user == null)
                return null;

            // authentication successful so generate jwt token
            var tokenHandler = new JwtSecurityTokenHandler();
            
            var key = Encoding.ASCII.GetBytes(_appSettings.Secret);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[] 
                {
                    new Claim(ClaimTypes.Name, user.Id.ToString())
                }),
                Expires = DateTime.UtcNow.AddDays(7),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            user.Token = tokenHandler.WriteToken(token);

            // remove password before returning
            //user.Password = null;

            return user;
        }
        
    }
}