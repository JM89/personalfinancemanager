using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using PFM.Authentication.Api.DTOs;
using PFM.Authentication.Api.Entities;
using PFM.Authentication.Api.Helpers;
using PFM.Authentication.Api.Models;
using PFM.Authentication.Api.Repositories.Interfaces;
using PFM.Authentication.Api.Services.Interfaces;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace PFM.Authentication.Api.Services
{
    internal class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly ISecretManagerService _secretManagerService;

        private const string PasswordSalt = "pfm/pwdsalt";

        private readonly AppSettings _appSettings;

        public UserService(IOptions<AppSettings> appSettings, IUserRepository userRepository, ISecretManagerService secretManagerService)
        {
            _appSettings = appSettings.Value;
            _userRepository = userRepository;
            _secretManagerService = secretManagerService;
        }

        public async Task<UserResponse> AuthenticateAsync(string username, string password)
        {
            var user = _userRepository.GetUserByName(username); 

            if (user == null)
                return null;

            var inputPassword = await GenerateSaltedHashAsync(password);
            if (!inputPassword.SequenceEqual(user.Password))
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
            return new UserResponse()
            {
                Id = user.Id,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Username = user.Username,
                Token = tokenHandler.WriteToken(token)
            };
        }

        public async Task<bool> CreateAsync(UserRequest userRequest)
        {
            var usernameAlreadyInUse = _userRepository.GetUserByName(userRequest.Username);
            if (usernameAlreadyInUse != null)
            {
                return false;
            }

            var user = new User()
            {
                FirstName = userRequest.FirstName,
                LastName = userRequest.LastName,
                Password = await GenerateSaltedHashAsync(userRequest.Password),
                Username = userRequest.Username
            };

            _userRepository.Create(user);
            return true;
        }

        private async Task<byte[]> GenerateSaltedHashAsync(string password)
        {
            SecretPasswordSaltModel result = null;
            try
            {
                result = await _secretManagerService.GetSecrets<SecretPasswordSaltModel>(PasswordSalt);
            }
            catch(Exception ex)
            {
                // TODO: add logging
                throw;  
            }

            if (result == null)
                throw new InvalidOperationException("Password Salt Secret could not be found");

            var saltStr = result.PasswordSalt;
            var plainText = Encoding.ASCII.GetBytes(password);
            var salt = Encoding.ASCII.GetBytes(saltStr);

            HashAlgorithm algorithm = new SHA256Managed();

            byte[] plainTextWithSaltBytes =
              new byte[plainText.Length + salt.Length];

            for (int i = 0; i < plainText.Length; i++)
            {
                plainTextWithSaltBytes[i] = plainText[i];
            }
            for (int i = 0; i < salt.Length; i++)
            {
                plainTextWithSaltBytes[plainText.Length + i] = salt[i];
            }

            return algorithm.ComputeHash(plainTextWithSaltBytes);
        }

        public Task<UserResponse> GetAuthenticatedUser(ClaimsIdentity identity)
        {
            int userId = Convert.ToInt32(identity.Name);
            var authenticatedUser = _userRepository.GetById(userId);

            return Task.FromResult(new UserResponse()
            {
                Id = authenticatedUser.Id,
                FirstName= authenticatedUser.FirstName,
                LastName = authenticatedUser.LastName,
                Username = authenticatedUser.Username
            });
        }
    }
}