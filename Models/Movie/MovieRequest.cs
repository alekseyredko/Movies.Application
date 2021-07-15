using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Movies.Application.Models.Movie
{
    public class MovieRequest
    {
        public string MovieName { get; set; }
        public TimeSpan? Duration { get; set; }
    }
}
