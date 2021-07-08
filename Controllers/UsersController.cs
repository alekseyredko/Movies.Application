using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using System.Threading.Tasks;
using System.Xml;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Movies.Application.Authentication;
using Movies.Application.Models;
using Movies.Application.Services;
using Movies.Data.Models;
using Movies.Data.Results;
using Movies.Data.Services.Interfaces;
using Movies.Application.Extensions;
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
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> LoginAsync(LoginUserRequest userRequest)
        {
            var user = _mapper.Map<LoginUserRequest, User>(userRequest);

            var result = await _userService.LoginAsync(user);
            
            var response =
                _mapper.Map<Result<User>, Result<LoginUserResponse>>(result);

            switch (result.ResultType)
            {
                case ResultType.Ok:
                    result.Value.Token = TokenHelper.GenerateJWTAsync(result.Value, _authConfiguration);
                    return Ok(response);

                default:
                    return this.ReturnFromResponse(response);
            }
        }

        // POST api/<UsersController>
        [HttpPost("register")]
        [AllowAnonymous]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> RegisterAsync(RegisterUserRequest userRequest)
        {
            var user = _mapper.Map<RegisterUserRequest, User>(userRequest);

            var result = await _userService.RegisterAsync(user);

            var response =
                _mapper.Map<Result<User>, Result<RegisterUserResponse>>(result);

            switch (response.ResultType)
            {
                case ResultType.Ok:
                    response.Value.Token = TokenHelper.GenerateJWTAsync(result.Value, _authConfiguration);
                    return Ok(response);

                default:
                    return this.ReturnFromResponse(response);
            }
        }
    }
}
