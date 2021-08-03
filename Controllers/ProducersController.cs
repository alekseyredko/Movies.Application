
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Movies.Infrastructure.Authentication;
using Movies.Infrastructure.Extensions;
using Movies.Infrastructure.Models;
using Movies.Infrastructure.Models.Producer;
using Movies.Infrastructure.Models.Reviewer;
using Movies.Infrastructure.Services;
using Movies.Data.Models;
using Movies.Data.Results;
using Movies.Data.Services.Interfaces;

namespace Movies.Infrastructure.Controllers
{
    namespace Movies.Infrastructure.Controllers
    {
        //TODO: validate entities
        [Route("api/[controller]")]
        [ApiController]
        public class ProducersController : ControllerBase
        {
            private readonly IProducerService _producerService;
            private readonly IMapper _mapper;
            private readonly AuthConfiguration _authConfiguration;
            private readonly IUserService _userService;

            public ProducersController(IProducerService producerService, IMapper mapper, IOptions<AuthConfiguration> authConfiguration, IUserService userService)
            {
               
                _mapper = mapper;
                _userService = userService;
                _producerService = producerService;
                _authConfiguration = authConfiguration.Value;
            }


            /// <summary>
            /// Get all Reviewers from database
            /// </summary>
            /// <returns>List of Reviewers</returns>
            /// <response code="200">Returns list of reviewers</response>
            [HttpGet]
            [AllowAnonymous]
            [ProducesResponseType(StatusCodes.Status200OK)]
            [ProducesResponseType(StatusCodes.Status400BadRequest)]
            public async Task<IActionResult> GetProducersAsync()
            {
                var reviewers = await _producerService.GetAllProducersAsync();
                var result = _mapper.Map<Result<IEnumerable<Producer>>, Result<IEnumerable<ProducerResponse>>>(reviewers);

                switch (result.ResultType)
                {
                    case ResultType.Ok:
                        return Ok(result);

                    default:
                        return this.ReturnFromResponse(result);
                }
            }

            [HttpGet("{id}")]
            [AllowAnonymous]
            [ProducesResponseType(StatusCodes.Status200OK)]
            [ProducesResponseType(StatusCodes.Status404NotFound)]
            public async Task<IActionResult> GetProducerAsync(int id)
            {
                var producer = await _producerService.GetProducerAsync(id);
                var result = _mapper.Map<Result<Producer>, Result<ProducerResponse>>(producer);

                switch (result.ResultType)
                {
                    case ResultType.Ok:
                        return Ok(result);

                    default:
                        return this.ReturnFromResponse(result);
                }
            }

            [HttpGet("account")]
            [Authorize]
            [ProducesResponseType(StatusCodes.Status200OK)]
            [ProducesResponseType(StatusCodes.Status404NotFound)]
            public async Task<IActionResult> GetReviewerAsync()
            {
                var id = TokenHelper.GetIdFromToken(HttpContext);
                var producer = await _producerService.GetProducerAsync(id);
                var result = _mapper.Map<Result<Producer>, Result<ProducerResponse>>(producer);

                switch (result.ResultType)
                {
                    case ResultType.Ok:
                        return Ok(result);

                    default:
                        return this.ReturnFromResponse(result);
                }
            }

            // POST api/<ReviewersController>
            [HttpPost("account/register")]
            [Authorize]
            [ProducesResponseType(StatusCodes.Status200OK)]
            [ProducesResponseType(StatusCodes.Status400BadRequest)]
            [ProducesResponseType(StatusCodes.Status401Unauthorized)]
            public async Task<IActionResult> PostProducerAsync(ProducerRequest request)
            {
                var mapped = _mapper.Map<ProducerRequest, Producer>(request);
                var id = TokenHelper.GetIdFromToken(HttpContext);

                mapped.ProducerId = id;

                var reviewer = await _producerService.AddProducerAsync(mapped);

                var result = _mapper.Map<Result<Producer>, Result<RegisterProducerResponse>>(reviewer);

                switch (result.ResultType)
                {
                    case ResultType.Ok:

                        var roles = await _userService.GetUserRolesAsync(id);
                        result.Value.Token = TokenHelper.GenerateJWTAsync(id, _authConfiguration, roles.ToArray());
                        return Ok(result);

                    default:
                        return this.ReturnFromResponse(result);
                }

            }

            // PUT api/<ReviewersController>/5
            [HttpPut("account/update")]
            [Authorize]
            [ProducesResponseType(StatusCodes.Status200OK)]
            [ProducesResponseType(StatusCodes.Status401Unauthorized)]
            [ProducesResponseType(StatusCodes.Status404NotFound)]
            public async Task<IActionResult> PutProducerAsync(ProducerRequest request)
            {
                var mapped = _mapper.Map<ProducerRequest, Producer>(request);

                var id = TokenHelper.GetIdFromToken(HttpContext);
                mapped.ProducerId = id;

                var updated = await _producerService.UpdateProducerAsync(mapped);
                var result = _mapper.Map<Result<Producer>, Result<ProducerResponse>>(updated);

                switch (result.ResultType)
                {
                    case ResultType.Ok:
                        return Ok(result);

                    default:
                        return this.ReturnFromResponse(result);
                }
            }

            [HttpDelete("account/delete")]
            [Authorize]
            [ProducesResponseType(StatusCodes.Status200OK)]
            [ProducesResponseType(StatusCodes.Status400BadRequest)]
            [ProducesResponseType(StatusCodes.Status401Unauthorized)]
            [ProducesResponseType(StatusCodes.Status404NotFound)]
            public async Task<IActionResult> DeleteAccountAsync()
            {
                var id = TokenHelper.GetIdFromToken(HttpContext);

                var result = await _producerService.DeleteProducerAsync(id);

                var response = _mapper.Map<Result, Result<TokenResponse>>(result);
                var roles = await _userService.GetUserRolesAsync(id);

                response.Value = new TokenResponse
                {
                    Token = TokenHelper.GenerateJWTAsync(id, _authConfiguration, roles.ToArray())
                };

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
}
