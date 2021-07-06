using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Movies.Application.Authentication;
using Movies.Data.Models;
using Movies.Data.Services.Interfaces;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Movies.Application.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly AuthConfiguration _authConfiguration;

        public UsersController(IUserService userService, IOptions<AuthConfiguration> authConfiguration)
        {
            _userService = userService;
            _authConfiguration = authConfiguration.Value;
        }

        // GET api/<UsersController>/5
        [HttpPost("login")]
        [AllowAnonymous]
        public async Task<IActionResult> LoginAsync(UserRequest userRequest)
        {
            try
            {
                var result = await _userService.LoginAsync(userRequest);
                result.Token = GenerateJWTAsync(result);
                return Ok(result);
            }
            catch (InvalidOperationException e)
            {
                return BadRequest(e.Message);
            }
        }

        // POST api/<UsersController>
        [HttpPost("register")]
        [AllowAnonymous]
        public async Task<IActionResult> RegisterAsync(UserRequest userRequest)
        {
            try
            {
                var userResponse = await _userService.RegisterAsync(userRequest);
                userResponse.Token = GenerateJWTAsync(userResponse);
                return Ok(userResponse);
            }
            catch (InvalidOperationException e)
            {
                return BadRequest(e.Message);
            }
        }

        private string GenerateJWTAsync(UserResponse user)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(_authConfiguration.Secret));

            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var claims = new List<Claim>()
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
            };

            var token = new JwtSecurityToken(
                _authConfiguration.Issuer,
                _authConfiguration.Audience,
                claims,
                expires: DateTime.Now.AddMinutes(_authConfiguration.TokenLifeTimeInMinutes),
                signingCredentials: credentials);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        //private Guid GetIdFromToken()
        //{
        //    var id = _contextAccessor
        //        .HttpContext
        //        .User
        //        .Claims
        //        .First(x => x.Type == ClaimTypes.NameIdentifier)
        //        .Value;

        //    return Guid.Parse(id);
        //}
    }
}
