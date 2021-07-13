﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Movies.Application.Models.Movie;
using Movies.Data.Models;
using Movies.Data.Results;

namespace Movies.Application.MappingProfiles
{
    public class MovieMappingProfile: Profile
    {
        public MovieMappingProfile()
        {
            CreateMap<MovieRequest, Movie>();
            CreateMap<UpdateMovieRequest, Movie>();

            CreateMap<Movie, MovieResponse>();
            CreateMap<Result<Movie>, Result<MovieResponse>>();
            CreateMap<Result<IEnumerable<Movie>>, Result<IEnumerable<MovieResponse>>>();
        }
    }
}
