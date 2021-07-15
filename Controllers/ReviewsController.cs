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
using Movies.Application.Extensions;
using Movies.Application.Models.Movie;
using Movies.Application.Models.Review;
using Movies.Application.Services;
using Movies.Data.Models;
using Movies.Data.Results;
using Movies.Data.Services.Interfaces;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Movies.Application.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReviewsController : ControllerBase
    {
        private readonly IReviewService _reviewService;
        private readonly IMapper _mapper;

        public ReviewsController(IReviewService reviewService, IMapper mapper)
        {
            _reviewService = reviewService;
            _mapper = mapper;
        }
        
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetReviewsAsync()
        {
            var reviews = await _reviewService.GetAllReviewsAsync();
            var result = _mapper.Map<Result<IEnumerable<Review>>, Result<IEnumerable<ReviewResponse>>>(reviews);

            switch (result.ResultType)
            {
                case ResultType.Ok:
                    return Ok(result);

                default:
                    return this.ReturnFromResponse(result);
            }
        }

        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetReviewByIdAsync(int id)
        {
            var review = await _reviewService.GetReviewAsync(id);
            var result = _mapper.Map<Result<Review>, Result<ReviewResponse>>(review);

            switch (result.ResultType)
            {
                case ResultType.Ok:
                    return Ok(result);

                default:
                    return this.ReturnFromResponse(result);
            }
        }

        [HttpPost]
        [Authorize(Roles = "Reviewer")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> PostReviewAsync(int id, [FromBody] ReviewRequest request)
        {
            var review = _mapper.Map<ReviewRequest, Review>(request);
            var reviewerId = TokenHelper.GetIdFromToken(HttpContext);

            var added = await _reviewService.AddReviewAsync(id, reviewerId, review);
            var result = _mapper.Map<Result<Review>, Result<ReviewResponse>>(added);

            switch (result.ResultType)
            {
                case ResultType.Ok:
                    return Ok(result);

                default:
                    return this.ReturnFromResponse(result);
            }
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "Reviewer")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> PutReviewAsync(int id, [FromBody] UpdateReviewRequest request)
        {
            var review = _mapper.Map<UpdateReviewRequest, Review>(request);
            var producerId = TokenHelper.GetIdFromToken(HttpContext);

            var added = await _reviewService.UpdateReviewAsync(producerId, id, review);
            var result = _mapper.Map<Result<Review>, Result<ReviewResponse>>(added);

            switch (result.ResultType)
            {
                case ResultType.Ok:
                    return Ok(result);

                default:
                    return this.ReturnFromResponse(result);
            }
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DeleteReviewAsync(int id)
        {
            var reviewerId = TokenHelper.GetIdFromToken(HttpContext);

            var response = await _reviewService.DeleteReviewAsync(reviewerId, id);

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
