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
        public void CreateToken()
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

            token.Should().NotBeNull("Token not be null");
        }
    }
}