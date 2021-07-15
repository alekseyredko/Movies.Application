using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Movies.Application.Models.Review;
using Movies.Application.Models.Reviewer;
using Movies.Data.Models;
using Movies.Data.Results;

namespace Movies.Application.MappingProfiles
{
    public class ReviewMappingProfile: Profile
    {
        public ReviewMappingProfile()
        {
            CreateMap<UpdateReviewRequest, Review>();
            CreateMap<ReviewRequest, Review>();

            CreateMap<Review, ReviewResponse>();
            CreateMap<Result<Review>, Result<ReviewResponse>>();

            CreateMap<IEnumerable<Review>, IEnumerable<ReviewResponse>>();
            CreateMap<Result<IEnumerable<Review>>, Result<IEnumerable<ReviewResponse>>>();
        }
    }
}
