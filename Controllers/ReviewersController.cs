using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.Data.SqlClient;
using Movies.Application.Filters;
using Movies.Application.Services;
using Movies.Data.Models;
using Movies.Data.Services.Interfaces;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Movies.Application.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReviewersController : ControllerBase
    {
        private readonly IReviewService _reviewService;
        private readonly IPersonService _personService;

        public ReviewersController(IReviewService reviewService, IPersonService personService)
        {
            _reviewService = reviewService;
            _personService = personService;
        }

        
        /// <summary>
        /// Get all Reviewers from database
        /// </summary>
        /// <returns>List of Reviewers</returns>
        /// <response code="200">Returns list of reviewers</response>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> GetReviewersAsync()
        {
            var reviewers = await _reviewService.GetAllReviewersAsync();
            return Ok(reviewers);
        }

        // GET api/<ReviewersController>/5
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ActionName("GetReviewerAsync")]
        public async Task<IActionResult> GetReviewerAsync(int id)
        {
            try
            {
                var reviewer = await _reviewService.GetReviewerAsync(id);
                return Ok(reviewer);
            }
            catch (InvalidOperationException e)
            {
                return NotFound();
            }
        }

        // GET api/<ReviewersController>/5
        [HttpGet("{id}/person")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetReviewerPersonAsync(int id)
        {
            try
            {
                var person = await _personService.GetPersonAsync(id);
                return Ok(person);
            }
            catch (InvalidOperationException e)
            {
                return NotFound();
            }
        }

        [HttpGet("{id}/reviews")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetReviewersReviewsAsync(int id)
        {
            try
            {
                var reviewer = await _reviewService.GetReviewerWithAllAsync(id);
                return Ok(reviewer.Reviews);
            }
            catch (InvalidOperationException e)
            {
                return NotFound();
            }
        }

        // POST api/<ReviewersController>
        [HttpPost]
        [Authorize]
        [ServiceFilter(typeof(ReviewerFilterAttribute))]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> PostReviewerAsync(
            [CustomizeValidator(RuleSet = "PostReviewer,other")] Reviewer reviewer)
        {
            try
            {
                await _reviewService.AddReviewerAsync(reviewer);
                return CreatedAtAction(nameof(GetReviewerAsync), new {id = reviewer.ReviewerId}, reviewer);
            }
            catch (InvalidOperationException e)
            {
                return BadRequest();
            }
        }

        // PUT api/<ReviewersController>/5
        [HttpPut()]
        [Authorize]
        [ServiceFilter(typeof(ReviewerFilterAttribute))]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> PutReviewerAsync(
            [CustomizeValidator(RuleSet = "PutReviewer,other")] Reviewer reviewer)
        {
            try
            {
                await _reviewService.UpdateReviewerAsync(reviewer);
                return NoContent();
            }
            catch (SqlException e)
            {
                return NotFound();
            }
        }

        // DELETE api/<ReviewersController>/5
        [HttpDelete]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DeleteReviewerAsync()
        {
            try
            {
                var id = TokenHelper.GetIdFromToken(HttpContext);
                await _reviewService.DeleteReviewerAsync(id);
                return NoContent();
            }
            catch (InvalidOperationException e)
            {
                return NotFound();
            }
        }

        // DELETE api/<ReviewersController>/5
        [HttpDelete("{id}")]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DeleteReviewerAsync(int id)
        {
            try
            {
                await _reviewService.DeleteReviewerAsync(id);
                return NoContent();
            }
            catch (InvalidOperationException e)
            {
                return NotFound();
            }
        }
    }
}
