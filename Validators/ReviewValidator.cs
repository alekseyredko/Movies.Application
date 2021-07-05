using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentValidation;
using Movies.Data.Models;

namespace Movies.Application.Validators
{
    public class ReviewValidator: AbstractValidator<Review>
    {
        public ReviewValidator()
        {
            RuleSet("PostReview", () =>
            {
                RuleFor(x => x.ReviewId).Null();
            });

            RuleSet("PutReview", () =>
            {
                RuleFor(x => x.ReviewId).NotEmpty();
            });

            RuleFor(x => x.LastUpdate).Null();
            RuleFor(x => x.Movie).Null();
            RuleFor(x => x.Reviewer).Null();
            RuleFor(x => x.MovieId).NotNull();
            RuleFor(x => x.Rate)
                .GreaterThanOrEqualTo(1)
                .LessThanOrEqualTo(10);
            RuleFor(x => x.RevievText).NotEmpty();
            RuleFor(x => x.ReviewerId).NotNull();
        }
    }
}
