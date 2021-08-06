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
using Movies.Infrastructure.Authentication;
using Movies.Infrastructure.Models;
using Movies.Infrastructure.Services;
using Movies.Data.Models;
using Movies.Data.Results;
using Movies.Data.Services.Interfaces;
using Movies.Infrastructure.Extensions;
using Movies.Infrastructure.Models.User;
using Movies.Infrastructure.Services.Interfaces;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Movies.Infrastructure.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly AuthConfiguration _authConfiguration;
        private readonly IMapper _mapper;
        private readonly IRefreshTokenService refreshTokenService;

        public UsersController(IUserService userService, 
                               IOptions<AuthConfiguration> authConfiguration, 
                               IMapper mapper, 
                               IRefreshTokenService refreshTokenService)
        {
            _userService = userService;
            _mapper = mapper;
            _authConfiguration = authConfiguration.Value;
            this.refreshTokenService = refreshTokenService;
        }

        [HttpGet("account")]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]       
        public async Task<IActionResult> GetUserAccountAsync()
        {
            var id = RefreshTokenService.GetIdFromToken(HttpContext);
            var user = await _userService.GetUserAccountAsync(id);
            var result = _mapper.Map<Result<User>, Result<GetUserResponse>>(user);

            switch (result.ResultType)
            {
                case ResultType.Ok:
                    return Ok(result);

                default:
                    return this.ReturnFromResponse(result);
            }
        }

        [HttpPost("account/login")]
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

            switch (response.ResultType)
            {
                case ResultType.Ok:

                    var tokens = await refreshTokenService.GenerateTokenPairAsync(result.Value.UserId);

                    response.Value.Token = tokens.Value.Token;
                    response.Value.RefreshToken = tokens.Value.RefreshToken;

                    return Ok(response);

                default:
                    return this.ReturnFromResponse(response);
            }
        }

        // POST api/<UsersController>
        [HttpPost("account/register")]
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

                    var tokens = await refreshTokenService.GenerateTokenPairAsync(result.Value.UserId);

                    response.Value.Token = tokens.Value.Token;
                    response.Value.RefreshToken = tokens.Value.RefreshToken;

                    return Ok(response);

                default:
                    return this.ReturnFromResponse(response);
            }
        }

        [HttpPut("account/update")]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> UpdateAccountAsync(UpdateUserRequest userRequest)
        {
            //TODO: validate requests
            var user = _mapper.Map<UpdateUserRequest, User>(userRequest);

            user.UserId = RefreshTokenService.GetIdFromToken(HttpContext);

            var result = await _userService.UpdateAccountAsync(user);

            var response =
                _mapper.Map<Result<User>, Result<UpdateUserResponse>>(result);

            switch (response.ResultType)
            {
                case ResultType.Ok:
                    
                    var tokens = await refreshTokenService.GenerateTokenPairAsync(result.Value.UserId);

                    response.Value.Token = tokens.Value.Token;                    
                    response.Value.RefreshToken = tokens.Value.RefreshToken;
                    
                    return Ok(response);

                default:
                    return this.ReturnFromResponse(response);
            }
        }

        [HttpDelete("account/delete")]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> DeleteAccountAsync()
        {
            var id = RefreshTokenService.GetIdFromToken(HttpContext);

            var response = await _userService.DeleteAccountAsync(id);

            switch (response.ResultType)
            {
                case ResultType.Ok:
                    return Ok(response);

                default:
                    return this.ReturnFromResponse(response);
            }
        }

        [HttpPost("account/logout")]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> LogoutAsync()
        {
            var response = await refreshTokenService.DeleteCookiesFromClient(Response);

            switch (response.ResultType)
            {
                case ResultType.Ok:
                    return Ok(response);

                default:
                    return this.ReturnFromResponse(response);
            }
        }

        //TODO: store refresh token in cookies
        [HttpPost("refresh-token")]
        [AllowAnonymous]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> RefreshTokenAsync(string token)
        {
            var result = await refreshTokenService.RefreshTokenAsync(token);

            switch (result.ResultType)
            {
                case ResultType.Ok:
                    return Ok(result);

                default:
                    return this.ReturnFromResponse(result);
            }
        }
    }
}
