using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.Extensions.DependencyInjection;
using Movies.Application.Validators;
using Movies.Data.Models;

namespace Movies.Application.Extensions
{
    public static class ValidationExtensions
    {
        public static void AddValidationExtensions(this IMvcBuilder mvcBuilder)
        {
            mvcBuilder.AddFluentValidation();
        }

        public static void RegisterValidators(this IServiceCollection serviceCollection)
        {
            serviceCollection.AddTransient<IValidator<Review>, ReviewValidator>();
            serviceCollection.AddTransient<IValidator<Reviewer>, ReviewerValidator>();
            serviceCollection.AddTransient<IValidator<Person>, PersonValidator>();
        }
    }
}
