using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Movies.Application.Models.Reviewer
{
    public class RegisterReviewerResponse: ReviewerResponse
    {
        public string Token { get; set; }
    }
}
