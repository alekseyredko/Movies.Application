using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Movies.Application.Authentication;
using Movies.Application.Models;
using Movies.Application.Services;
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
        private readonly IMapper _mapper;

        public UsersController(IUserService userService, IOptions<AuthConfiguration> authConfiguration, IMapper mapper)
        {
            _userService = userService;
            _mapper = mapper;
            _authConfiguration = authConfiguration.Value;
        }

        [HttpPost("login")]
        [AllowAnonymous]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> LoginAsync(LoginUserRequest userRequest)
        {
            try
            {
                var user = _mapper.Map<LoginUserRequest, User>(userRequest);

                var result = await _userService.LoginAsync(user);
                result.Token = TokenHelper.GenerateJWTAsync(result, _authConfiguration);

                var response = _mapper.Map<User, LoginUserResponse>(result);

                return Ok(response);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        // POST api/<UsersController>
        [HttpPost("register")]
        [AllowAnonymous]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> RegisterAsync(RegisterUserRequest userRequest)
        {
            try
            {
                var user = _mapper.Map<RegisterUserRequest, User>(userRequest);

                var result = await _userService.RegisterAsync(user);
                result.Token = TokenHelper.GenerateJWTAsync(result, _authConfiguration);

                var response = _mapper.Map<User, RegisterUserResponse>(result);

                return Ok(response);
            }
            catch (InvalidOperationException e)
            {
                return BadRequest(e.Message);
            }
        }
    }
}
