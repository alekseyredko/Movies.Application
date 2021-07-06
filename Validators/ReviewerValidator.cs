using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentValidation;
using Microsoft.AspNetCore.DataProtection.Repositories;
using Movies.Data.Models;

namespace Movies.Application.Validators
{
    public class ReviewerValidator: AbstractValidator<Reviewer>
    {
        public ReviewerValidator()
        {
            RuleSet("PostReviewer", () =>
            {
                RuleFor(x => x.ReviewerId).Empty();
                RuleFor(x => x.Person)
                    .SetValidator(new PersonValidator(), "PostPerson", "other");
            });

            RuleSet("PutReviewer", () =>
            {
                
                RuleFor(x => x.ReviewerId).NotEmpty();
            });

            RuleSet("other", () =>
            {
                RuleForEach(x => x.Movies).Null();
                RuleForEach(x => x.ReviewerWatchHistories).Null();
                RuleForEach(x => x.Reviews).Null();
            });
        }
    }
}
