using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Text;

namespace DeliveryService.Api.Controllers
{
    [Route("api/[controller]"), ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IConfiguration _config;

        public AuthController(IConfiguration config)
        {
            _config = config;
        }

        [HttpPost, AllowAnonymous]
        public IActionResult CreateToken([FromBody] LoginModel login)
        {
            try
            {
                UserModel user = Authenticate(login);

                if (user != null)
                {
                    return Ok(new { token = BuildToken(user) });
                }

                return Unauthorized();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        private string BuildToken(UserModel user)
        {
            SymmetricSecurityKey key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));
            SigningCredentials creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            JwtSecurityToken token = new JwtSecurityToken(
                _config["Jwt:Issuer"],
                _config["Jwt:Issuer"],
                expires: DateTime.Now.AddMinutes(30),
                signingCredentials: creds
                );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        private UserModel Authenticate(LoginModel login) =>
            new UserModel { Name = login.Username, Role = login.Role };

        public class LoginModel
        {
            public string Username { get; set; }
            public string Password { get; set; }
            public string Role { get; set; }
        }

        public class UserModel
        {
            public string Name { get; set; }
            public string Role { get; set; }
        }
    }
}