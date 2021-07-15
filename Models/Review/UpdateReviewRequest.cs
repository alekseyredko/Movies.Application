using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Movies.Application.Models.Review
{
    public class UpdateReviewRequest
    {
        public int? Rate { get; set; }
        public string ReviewText { get; set; }
    }
}
