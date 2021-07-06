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
    public class MoviesController : ControllerBase
    {
        private readonly IMovieService _movieService;
        private readonly IReviewService _reviewService;
        private readonly IActorsService _actorsService;

        public MoviesController(IMovieService movieService, IActorsService actorsService, IReviewService reviewService)
        {
            _movieService = movieService;
            _actorsService = actorsService;
            _reviewService = reviewService;
        }

        // GET: api/<MoviesController>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> GetMoviesAsync()
        {
            var movies = await _movieService.GetAllMoviesAsync();
            return Ok(movies);
        }

        // GET api/<MoviesController>/5
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ActionName("GetMovieByIdAsync")]
        public async Task<IActionResult> GetMovieByIdAsync(int id)
        {
            try
            {
                var movie = await _movieService.GetMovieAsync(id);
                return Ok(movie);
            }
            catch (InvalidOperationException e)
            {
                return NotFound();
            }
        }

        [HttpGet("{id}/actors")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetActorsFromMovieAsync(int id)
        {
            try
            {
                var actors = await _movieService.GetMovieActorsAsync(id);
                return Ok(actors);
            }
            catch (InvalidOperationException e)
            {
                return NotFound();
            }
        }

        [HttpGet("{id}/reviews")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetReviewsFromMovieAsync(int id)
        {
            try
            {
                var movie = await _movieService.GetMovieWithReviewsAsync(id);
                return Ok(movie.Reviews);
            }
            catch (InvalidOperationException e)
            {
                return NotFound();
            }
        }

        // POST api/<MoviesController>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> PostMovieAsync(
            [CustomizeValidator(RuleSet = "PostMovie")]Movie movie)
        {
            try
            {
                await _movieService.AddMovieAsync(movie);
                return CreatedAtAction(nameof(GetMovieByIdAsync), new {id = movie.MovieId}, movie);
            }
            catch (InvalidOperationException e)
            {
                return BadRequest();
            }
        }


        // PUT api/<MoviesController>/5
        [HttpPut]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> PutMovieAsync(
            [CustomizeValidator(RuleSet = "PutMovie")] Movie movie)
        {
            try
            {
                await _movieService.UpdateMovieAsync(movie);
                return NoContent();
            }
            catch (SqlException e)
            {
                Console.WriteLine(e);
                return NotFound();
            }
        }

        // DELETE api/<MoviesController>/5
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DeleteMovieAsync(int id)
        {
            try
            {
                await _movieService.DeleteMovieAsync(id);
                return NoContent();
            }
            catch (InvalidOperationException e)
            {
                return NotFound();
            }
        }
    }
}
