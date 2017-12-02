using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using ForexSignals.Data.Configuration;
using ForexSignals.Data.Enums;
using ForexSignals.Data.Exceptions;
using ForexSignals.Data.Models;
using ForexSignals.Data.Requests;
using ForexSignals.Data.Responses;
using ForexSignals.DataAccess.Adapters;
using ForexSignals.DataAccess.Repositories;
using Microsoft.AspNetCore.Authorization.Infrastructure;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace ForexSignals.AuthServer.BusinessLogic
{
    public class UserActions
    {
        private readonly UserRepository _userRepository;
        private readonly AppSettings _config;

        public UserActions(IOptions<AppSettings> config)
        {
            var postgresConfig = config.Value.PostgresSettings;
            _config = config.Value;
            _userRepository = new UserRepository(new MartenAdapter(postgresConfig));
        }
        
        private string HashPassword(string text)
        {
            using (var hasher = SHA512.Create())
            {
                var textWithSaltBytes = Encoding.UTF8.GetBytes(string.Concat(text, _config.AuthenticationSettings.SecretKey));
                var hashedBytes = hasher.ComputeHash(textWithSaltBytes);
                return Convert.ToBase64String(hashedBytes);
            }
        }

        public async Task<UserResponse> AddUserAsync(NewUserRequest newUser)
        {
            var user = new User
            {
                Username = newUser.Username,
                Password = HashPassword(newUser.Password),
                Email = newUser.Email,
                Firstname = newUser.Firstname,
                Lastname = newUser.Lastname,
                TermsAccepted = newUser.TermsAccepted,
                UserType = UserType.Guest
            };
            user = await _userRepository.SaveUserAsync(user);
            return new UserResponse(user);
        }

        public async Task<bool> AuthenticateUserPasswordAsync(UserLoginRequest login)
        {
            var user = await _userRepository.GetUserByUsernameAsync(login.Username);
            var hashedPass = HashPassword(login.Password);
            return hashedPass != user.Password;
        }

        public JwtSecurityToken GetToken(string username)
        {
            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, username),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config.AuthenticationSettings.SecretKey));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512);

            var token = new JwtSecurityToken(
                issuer: _config.UrlSettings.AuthUrl,
                audience: _config.UrlSettings.AuthUrl,
                claims: claims,
                expires: DateTime.Now.AddHours(1),
                signingCredentials: creds
            );
            return token;
        }

        public async Task<UserResponse> GetUserByNameAsync(string username)
        {
            return new UserResponse(await _userRepository.GetUserByUsernameAsync(username));
        }
    }
}
