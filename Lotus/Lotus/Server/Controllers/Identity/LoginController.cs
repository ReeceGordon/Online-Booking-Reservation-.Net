using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Lotus.Shared.Identity;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace Lotus.Server.Controllers.Identity
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoginController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly SignInManager<ApplicationUser> _signInManager;

        public LoginController(IConfiguration configuration,
                               SignInManager<ApplicationUser> signInManager)
        {
            _configuration = configuration;
            _signInManager = signInManager;
        }

        [HttpPost]
        public async Task<IActionResult> Login([FromBody] LoginModel login)
        {
            var result = await _signInManager.PasswordSignInAsync(login.Email, login.Password, false, false);

            //Grab user by email
            var user = await _signInManager.UserManager.FindByEmailAsync(login.Email);

            //Email validation error message population
            //Confirms user has entered correct login detials in order to show specific error message
            //Else shows generic Invalid Login Attempt below to give no information to malicious hackers.
            if (!result.Succeeded && user != null && !user.EmailConfirmed && await _signInManager.UserManager.CheckPasswordAsync(user, login.Password)) return BadRequest(new LoginResult { Successful = false, Error = "Email Validation required to sign in." });

            if (!result.Succeeded) return BadRequest(new LoginResult { Successful = false, Error = "Invalid Login Attempt." });

            //grab roles associated with said user
            var roles = await _signInManager.UserManager.GetRolesAsync(user);


            /*
            //Old claims
            var claims = new[]
            {
            new Claim(ClaimTypes.Name, login.Email)
            };
            */

            var claims = new List<Claim>();

            //Base claim data as previously done in old claims code
            claims.Add(new Claim(ClaimTypes.Name, login.Email));

            //Aditional claim to add roles to the claims
            //Important to NOTE!!!
            //A quirk of role Claims, if a user is in two roles then two role claims are not added as you would expect.
            //Instead the role claims get combined into an array of role claims.
            //This means in our AuthService we will do some extra work to determine if a user has multiple roles or not
            foreach (var role in roles)
            {
                claims.Add(new Claim(ClaimTypes.Role, role));
            }


            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JwtSecurityKey"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var expiry = DateTime.Now.AddDays(Convert.ToInt32(_configuration["JwtExpiryInDays"]));

            var token = new JwtSecurityToken(
                _configuration["JwtIssuer"],
                _configuration["JwtAudience"],
                claims,
                expires: expiry,
                signingCredentials: creds
            );

            return Ok(new LoginResult { Successful = true, Token = new JwtSecurityTokenHandler().WriteToken(token) });
        }

    }
}