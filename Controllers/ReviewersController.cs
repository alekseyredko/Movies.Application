using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Options;
using Movies.Infrastructure.Authentication;
using Movies.Infrastructure.Extensions;
using Movies.Infrastructure.Filters;
using Movies.Infrastructure.Models;
using Movies.Infrastructure.Models.Reviewer;
using Movies.Infrastructure.Services;
using Movies.Data.Models;
using Movies.Data.Results;
using Movies.Data.Services.Interfaces;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Movies.Infrastructure.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    //TODO: validate entities
    public class ReviewersController : ControllerBase
    {
        private readonly IReviewService _reviewService;
        private readonly IPersonService _personService;
        private readonly IUserService _userService;
        private readonly IMapper _mapper;
        private readonly AuthConfiguration _authConfiguration;

        public ReviewersController(IReviewService reviewService, IPersonService personService, IMapper mapper,
            IUserService userService, IOptions<AuthConfiguration> authConfiguration)
        {
            _reviewService = reviewService;
            _personService = personService;
            _mapper = mapper;
            _userService = userService;
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
        public async Task<IActionResult> GetReviewersAsync()
        {
            var reviewers = await _reviewService.GetAllReviewersAsync();
            var result = _mapper.Map<Result<IEnumerable<Reviewer>>, Result<IEnumerable<ReviewerResponse>>>(reviewers);

            switch (result.ResultType)
            {
                case ResultType.Ok:
                    return Ok(result);

                default:
                    return this.ReturnFromResponse(result);
            }
        }

        // GET api/<ReviewersController>/5
        [HttpGet("{id}")]
        [AllowAnonymous]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ActionName("GetReviewerAsync")]
        public async Task<IActionResult> GetReviewerAsync(int id)
        {
            var reviewer = await _reviewService.GetReviewerAsync(id);
            var result = _mapper.Map<Result<Reviewer>, Result<ReviewerResponse>>(reviewer);

            switch (result.ResultType)
            {
                case ResultType.Ok:
                    return Ok(result);

                default:
                    return this.ReturnFromResponse(result);
            }
        }

        // GET api/<ReviewersController>/
        [HttpGet("account")]
        [Authorize(Roles = "Reviewer")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetReviewerAsync()
        {
            var id = TokenHelper.GetIdFromToken(HttpContext);
            var reviewer = await _reviewService.GetReviewerAsync(id);
            var result = _mapper.Map<Result<Reviewer>, Result<ReviewerResponse>>(reviewer);

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
        public async Task<IActionResult> PostReviewerAsync(RegisterReviewerRequest request)
        {
            var mapped = _mapper.Map<RegisterReviewerRequest, Reviewer>(request);
            var id = TokenHelper.GetIdFromToken(HttpContext);

            mapped.ReviewerId = id;

            var reviewer = await _reviewService.AddReviewerAsync(mapped);

            var result = _mapper.Map<Result<Reviewer>, Result<RegisterReviewerResponse>>(reviewer);

            switch (result.ResultType)
            {
                case ResultType.Ok:

                    var roles = await _userService.GetUserRolesAsync(id);
                    result.Value.Token = TokenHelper.GenerateJWTAsync(id, roles, _authConfiguration);
                    return Ok(result);

                default:
                    return this.ReturnFromResponse(result);
            }

        }

        // PUT api/<ReviewersController>/5
        [HttpPut("account/update")]
        [Authorize(Roles = "Reviewer")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> PutReviewerAsync(ReviewerRequest request)
        {
            var mapped = _mapper.Map<ReviewerRequest, Reviewer>(request);

            var id = TokenHelper.GetIdFromToken(HttpContext);
            mapped.ReviewerId = id;

            var updated = await _reviewService.UpdateReviewerAsync(mapped);
            var result = _mapper.Map<Result<Reviewer>, Result<ReviewerResponse>>(updated);

            switch (result.ResultType)
            {
                case ResultType.Ok:
                    return Ok(result);

                default:
                    return this.ReturnFromResponse(result);
            }
        }

        [HttpDelete("account/delete")]
        [Authorize(Roles = "Reviewer")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DeleteAccountAsync()
        {
            var id = TokenHelper.GetIdFromToken(HttpContext);

            var result = await _reviewService.DeleteReviewerAsync(id);

            var response = _mapper.Map<Result, Result<TokenResponse>>(result);
            var roles = await _userService.GetUserRolesAsync(id);
            
            response.Value = new TokenResponse
            {
                Token = TokenHelper.GenerateJWTAsync(id, roles, _authConfiguration)
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
