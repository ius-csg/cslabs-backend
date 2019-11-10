using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using CSLabsBackend.Config;
using CSLabsBackend.Models;
using CSLabsBackend.Models.UserModels;
using CSLabsBackend.Util;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace CSLabsBackend.Services
{
    public interface IAuthenticationService
    {
        User Authenticate(string email, string password);
        Task<User> AddUser(User user);
    }

    public class AuthenticationService : IAuthenticationService
    {
        private readonly AppSettings _appSettings;
        private readonly DefaultContext _databaseContext;

        public AuthenticationService(AppSettings appSettings, DefaultContext defaultContext)
        {
            _appSettings = appSettings;
            _databaseContext = defaultContext;
        }

        public async Task<User> AddUser(User user)
        {
            var hasher = new PasswordHasher<User>();
            user.Password = hasher.HashPassword(user, user.Password);
            _databaseContext.Users.Add(user);
            await _databaseContext.SaveChangesAsync();
            return user;
        }

        public User Authenticate(string email, string password)
        {
            // @todo authenticate with kerberos.
            var user = _databaseContext.Users
                .FirstOrDefault(x =>
                    (x.SchoolEmail == email || x.PersonalEmail == email));
            
            // return null if user not found
            if (user == null)
                return null;
            
            var hasher = new PasswordHasher<User>();
            if(hasher.VerifyHashedPassword(user, user.Password, password) == PasswordVerificationResult.Failed) {
                return null;
            }
            

            // authentication successful so generate jwt token
            var tokenHandler = new JwtSecurityTokenHandler();

            var key = Encoding.ASCII.GetBytes(_appSettings.JWTSecret);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
                    new Claim(ClaimTypes.Name, user.Id.ToString())
                }),
                Expires = DateTime.UtcNow.AddDays(7),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key),
                    SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            user.Token = tokenHandler.WriteToken(token);

            // remove password before returning
            user.Password = null;

            return user;
        }
        
    }
}