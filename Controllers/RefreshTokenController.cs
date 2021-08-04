using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Movies.Data.Models;
using Movies.Data.Results;
using Movies.Infrastructure.Extensions;
using Movies.Infrastructure.Models;
using Movies.Infrastructure.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Movies.Application.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RefreshTokenController : ControllerBase
    {
        private readonly ITokenUserService tokenUserService;
        private readonly IMapper mapper;

        public RefreshTokenController(ITokenUserService tokenUserService, IMapper mapper)
        {
            this.tokenUserService = tokenUserService;
            this.mapper = mapper;
        }

        [HttpPost("refresh")]
        [AllowAnonymous]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> RefreshTokenAsync(string token)
        {            
            var result = await tokenUserService.RefreshTokenAsync(token);

            var response =
                mapper.Map<Result<User>, Result<LoginUserResponse>>(result);

            switch (result.ResultType)
            {
                case ResultType.Ok:
                    return Ok(response);

                default:
                    return this.ReturnFromResponse(response);
            }
        }
    }
}
