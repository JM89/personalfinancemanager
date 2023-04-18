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
using System.Runtime.CompilerServices;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

[assembly: InternalsVisibleTo("PFM.Authentication.Api.Tests")]
namespace PFM.Authentication.Api.Services
{
    internal class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly IUserTokenRepository _userTokenRepository;
        private readonly ISecretManagerService _secretManagerService;
        private readonly Serilog.ILogger _logger;

        private const string PasswordSalt = "pfm/pwdsalt";

        private readonly AppSettings _appSettings;

        public UserService(IOptions<AppSettings> appSettings, IUserRepository userRepository, IUserTokenRepository userTokenRepository, ISecretManagerService secretManagerService, Serilog.ILogger logger)
        {
            _appSettings = appSettings.Value;
            _userRepository = userRepository;
            _userTokenRepository = userTokenRepository;
            _secretManagerService = secretManagerService;
            _logger = logger;
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

        public async Task<User> CreateAsync(UserRequest userRequest)
        {
            var usernameAlreadyInUse = _userRepository.GetUserByName(userRequest.Username);
            if (usernameAlreadyInUse != null)
            {
                return usernameAlreadyInUse;
            }

            var user = new User()
            {
                FirstName = userRequest.FirstName,
                LastName = userRequest.LastName,
                Password = await GenerateSaltedHashAsync(userRequest.Password),
                Username = userRequest.Username
            };

            _userRepository.Create(user);
            return user;
        }

        public async Task<byte[]> GenerateSaltedHashAsync(string password)
        {
            SecretPasswordSaltModel result = null;
            try
            {
                result = await _secretManagerService.GetSecrets<SecretPasswordSaltModel>(PasswordSalt);
            }
            catch(Exception ex)
            {
                _logger.Error(ex, $"Unhandled Exception: {ex.Message}");
                throw;  
            }

            if (result == null)
                throw new InvalidOperationException("Password Salt Secret could not be found");

            var saltStr = result.PasswordSalt;
            var plainText = Encoding.ASCII.GetBytes(password);
            var salt = Encoding.ASCII.GetBytes(saltStr);

            using (var alg = SHA512.Create())
            {
                return alg.ComputeHash(new byte[plainText.Length + salt.Length]);
            }
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

        public Task<bool> ValidateToken(ClaimsIdentity identity, string token)
        {
            int userId = Convert.ToInt32(identity.Name);
            var authenticatedUser = _userRepository.GetById(userId);

            return Task.FromResult(_userTokenRepository.ValidateToken(authenticatedUser.Username, token));
        }
    }
}