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
    public class ReviewsController : ControllerBase
    {
        private IReviewService _reviewService;

        public ReviewsController(IReviewService reviewService)
        {
            _reviewService = reviewService;
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> GetReviewsAsync()
        {
            var reviews = await _reviewService.GetAllReviewsAsync();
            return Ok(reviews);
        }

        // GET api/<ReviewsController>/5
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetReviewAsync(int id)
        {
            try
            {
                var review =  await _reviewService.GetReviewAsync(id);
                return Ok(review);
            }
            catch (InvalidOperationException e)
            {
                return NotFound();
            }
        }

        // POST api/<ReviewsController>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> PostReviewAsync(
            [CustomizeValidator(RuleSet="PostReview,other")] Review review)
        {
            try
            {
                await _reviewService.AddReviewAsync(review, review.MovieId);
                return Ok(review);
            }
            catch (InvalidOperationException e)
            {
                System.Diagnostics.Debug.WriteLine(e.Message);
                return BadRequest();
            }
        }

        // PUT api/<ReviewsController>/5
        [HttpPut]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> PutAsync(
            [CustomizeValidator(RuleSet = "PutReview,other")] Review review)
        {
            try
            {
                await _reviewService.UpdateReviewService(review);
                return NoContent();
            }
            catch (SqlException e)
            {
                return NotFound();
            }
        }

        // DELETE api/<ReviewsController>/5
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DeleteReviewAsync(int id)
        {
            try
            {
                await _reviewService.DeleteReviewAsync(id);
                return NoContent();
            }
            catch (InvalidOperationException e)
            {
                return NotFound();
            }
        }
    }
}
