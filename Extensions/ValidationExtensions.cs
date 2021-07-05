using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentValidation.AspNetCore;
using Microsoft.Extensions.DependencyInjection;

namespace Movies.Application.Extensions
{
    public static class ValidationExtensions
    {
        public static void AddValidationExtensions(this IMvcBuilder mvcBuilder)
        {
            mvcBuilder.AddFluentValidation(
                );
        }
    }
}
