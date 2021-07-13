﻿using Microsoft.AspNetCore.Mvc;
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
using Movies.Application.Models.User;

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


        // GET api/<ReviewersController>/
        [HttpGet("account")]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ActionName("GetReviewerAsync")]
        public async Task<IActionResult> GetReviewerAsync()
        {
            var id = TokenHelper.GetIdFromToken(HttpContext);
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

            switch (result.ResultType)
            {
                case ResultType.Ok:
                    response.Value.Token = TokenHelper.GenerateJWTAsync(result.Value, _authConfiguration);
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
                    response.Value.Token = TokenHelper.GenerateJWTAsync(result.Value, _authConfiguration);
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

            user.UserId = TokenHelper.GetIdFromToken(HttpContext);

            var result = await _userService.UpdateAccountAsync(user);

            var response =
                _mapper.Map<Result<User>, Result<UpdateUserResponse>>(result);

            switch (response.ResultType)
            {
                case ResultType.Ok:
                    response.Value.Token = TokenHelper.GenerateJWTAsync(result.Value, _authConfiguration);
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
            var id = TokenHelper.GetIdFromToken(HttpContext);

            var response = await _userService.DeleteAccountAsync(id);

            switch (response.ResultType)
            {
                case ResultType.Ok:
                    return Ok(response);

                default:
                    return this.ReturnFromResponse(response);
            }
        }
    }
}
