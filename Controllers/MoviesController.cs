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
using Movies.Application.Services;
using Movies.Data.Models;
using Movies.Data.Results;
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
        private IMapper _mapper;

        public MoviesController(IMovieService movieService, IActorsService actorsService, IReviewService reviewService, IMapper mapper)
        {
            _movieService = movieService;
            _actorsService = actorsService;
            _reviewService = reviewService;
            _mapper = mapper;
        }

        // GET: api/<MoviesController>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetMoviesAsync()
        {
            var movies = await _movieService.GetAllMoviesAsync();
            var result = _mapper.Map<Result<IEnumerable<Movie>>, Result<IEnumerable<MovieResponse>>>(movies);

            switch (result.ResultType)
            {
                case ResultType.Ok:
                    return Ok(result);

                default:
                    return this.ReturnFromResponse(result);
            }
        }

        // GET api/<MoviesController>/5
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetMovieByIdAsync(int id)
        {
            var movie = await _movieService.GetMovieAsync(id);
            var result = _mapper.Map<Result<Movie>, Result<MovieResponse>>(movie);

            switch (result.ResultType)
            {
                case ResultType.Ok:
                    return Ok(result);

                default:
                    return this.ReturnFromResponse(result);
            }
        }

        // POST api/<MoviesController>
        [HttpPost]
        [Authorize(Roles = "Producer")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> PostMovieAsync(MovieRequest request)
        {
            var movie = _mapper.Map<MovieRequest, Movie>(request);
            var id = TokenHelper.GetIdFromToken(HttpContext);

            var added = await _movieService.AddMovieAsync(id, movie);
            var result = _mapper.Map<Result<Movie>, Result<MovieResponse>>(added);

            switch (result.ResultType)
            {
                case ResultType.Ok:
                    return Ok(result);

                default:
                    return this.ReturnFromResponse(result);
            }
        }

        // PUT api/<MoviesController>/5
        [HttpPut("id")]
        [Authorize(Roles = "Producer")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> PutMovieAsync(int movieId, [FromBody] UpdateMovieRequest request)
        {
            var movie = _mapper.Map<UpdateMovieRequest, Movie>(request);
            var id = TokenHelper.GetIdFromToken(HttpContext);

            var added = await _movieService.UpdateMovieAsync(id, movieId, movie);
            var result = _mapper.Map<Result<Movie>, Result<MovieResponse>>(added);

            switch (result.ResultType)
            {
                case ResultType.Ok:
                    return Ok(result);

                default:
                    return this.ReturnFromResponse(result);
            }
        }

        // DELETE api/<MoviesController>/5
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DeleteMovieAsync(int id)
        {
            var producerId = TokenHelper.GetIdFromToken(HttpContext);

            var response = await _movieService.DeleteMovieAsync(producerId, id);

            switch (response.ResultType)
            {
                case ResultType.Ok:
                    return Ok(response);

                default:
                    return this.ReturnFromResponse(response);
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
    }
}
