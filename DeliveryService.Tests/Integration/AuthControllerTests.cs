using DeliveryService.Api.Controllers;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Xunit;

namespace DeliveryService.Tests
{
    public class AuthControllerTests
    {
        private readonly IConfiguration _config;
        public AuthControllerTests()
        {
            _config = new ConfigurationBuilder()
                .AddJsonFile(@"jwtsettings.json").Build();
        }

        [Fact]
        public void CreateToken_Token_NotBeNull()
        {
            AuthController controller = new AuthController(_config);

            Login login = new Login()
            {
                Username = "Username",
                Password = "Password",
                Role = "Role"
            };

            IActionResult result = controller.CreateToken(login);

            string token = ((ObjectResult)(result.Should().Subject)).Value.As<string>();

            token.Should().NotBeNull("Token cannot be null");
        }

        [Fact]
        public void CreateToken_StatusCode_200()
        {
            AuthController controller = new AuthController(_config);

            Login login = new Login()
            {
                Username = "Username",
                Password = "Password",
                Role = "Role"
            };

            IActionResult actionResult = controller.CreateToken(login);

            var statusCode = ((ObjectResult)(actionResult.Should().Subject)).StatusCode;

            statusCode.Should().Be(200, $"StatusCode should be {200}");
        }

        [Fact]
        public void CreateToken_StatusCode_401()
        {
            AuthController controller = new AuthController(_config);

            Login login = null;

            IActionResult actionResult = controller.CreateToken(login);

            var statusCode = ((UnauthorizedResult)(actionResult.Should().Subject)).StatusCode;

            statusCode.Should().Be(401, $"StatusCode should be {401}");
        }
    }
}