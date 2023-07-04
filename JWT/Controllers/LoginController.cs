using JWT.Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace JWT.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoginController : ControllerBase
    {
        private readonly IConfiguration _config;

        public LoginController(IConfiguration config)
        {
            _config = config;
        }

        // POST api/<LoginController>
        [HttpPost("authenticate"), AllowAnonymous]

        public IActionResult Post([FromQuery] string userName, [FromQuery] string passWord)
        {
            var user = Authenticate(userName, passWord);
            if (user != null)
            {
                var token = GenerateJwtToken(user);
                return Ok(token);
            }
            return BadRequest();
        }

        private object GenerateJwtToken(UserModel user)
        {
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["JWT:Key"]));
            var signature = new SigningCredentials(key, SecurityAlgorithms.HmacSha256Signature);
            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, user.UserName),
                new Claim(ClaimTypes.Email, user.EmailAddress),
                new Claim(ClaimTypes.Role, user.Role),
            };

            var token = new JwtSecurityToken(
                _config.GetSection("JWT:Issuer").Value,
                _config["JWT:Audience"],
                claims,
                expires: DateTime.Now.AddMinutes(15),
                signingCredentials: signature
                );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        private object JwtSecurityTokenHandler()
        {
            throw new NotImplementedException();
        }

        private UserModel Authenticate(string userName, string passWord)
        {
            if (!string.IsNullOrEmpty(userName) && !string.IsNullOrEmpty(passWord))
            {
                var curentUser = UserConstants.Users.FirstOrDefault(x => x.UserName.ToLower() == userName && x.PassWord.ToLower() == passWord);
                if (curentUser != null)
                {
                    return curentUser;
                }
            }
            return null;
        }
    }
}
