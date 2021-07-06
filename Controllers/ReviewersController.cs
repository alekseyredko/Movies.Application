using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Http;
using Microsoft.Data.SqlClient;
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

        // GET: api/<ReviewersController>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ActionName("GetReviewerAsync")]
        public async Task<IActionResult> GetReviewersAsync()
        {
            var reviewers = await _reviewService.GetAllReviewersAsync();
            return Ok(reviewers);
        }

        // GET api/<ReviewersController>/5
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
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
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
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
        [ProducesResponseType(StatusCodes.Status204NoContent)]
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
                Console.WriteLine(e);
                return NotFound();
            }
        }

        // DELETE api/<ReviewersController>/5
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
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
