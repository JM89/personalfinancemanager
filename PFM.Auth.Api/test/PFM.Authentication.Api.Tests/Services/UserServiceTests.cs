using Microsoft.Extensions.Options;
using Moq;
using PFM.Authentication.Api.Helpers;
using PFM.Authentication.Api.Models;
using PFM.Authentication.Api.Repositories.Interfaces;
using PFM.Authentication.Api.Services;
using PFM.Authentication.Api.Services.Interfaces;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace PFM.Authentication.Api.Tests.Services
{
    public class UserServiceTests
    {
        private Mock<IUserRepository> _userRepository;
        private Mock<IUserTokenRepository> _userTokenRepository;
        private Mock<ISecretManagerService> _secretManagerService;
        private Mock<Serilog.ILogger> _logger;
        private Mock<IOptions<AppSettings>> _appSettings;
        private UserService _service;

        public UserServiceTests()
        {
            _userRepository = new Mock<IUserRepository>();
            _userTokenRepository = new Mock<IUserTokenRepository>();
            _secretManagerService = new Mock<ISecretManagerService>();
            _secretManagerService.Setup(x => x.GetSecrets<SecretPasswordSaltModel>("pfm/pwdsalt")).ReturnsAsync(new SecretPasswordSaltModel() { PasswordSalt = "Salt" });
            _logger = new Mock<Serilog.ILogger>();
            _appSettings = new Mock<IOptions<AppSettings>>();
            _service = new UserService(_appSettings.Object, _userRepository.Object, _userTokenRepository.Object, _secretManagerService.Object, _logger.Object);
        }

        [Fact]
        public async Task WhenUserRegisterThenCreateUserWithGeneratedPasswordHash()
        {
            // arrange
            var request = new DTOs.UserRequest() { Username = "john", FirstName = "John", LastName = "Smith", Password = "SecurityMatters123!" };

            // act
            var result = await _service.CreateAsync(request);
            var inputPassword = await _service.GenerateSaltedHashAsync("SecurityMatters123!");

            // assert
            Assert.NotNull(result);
            Assert.True(inputPassword.SequenceEqual(result.Password));
        }
    }
}