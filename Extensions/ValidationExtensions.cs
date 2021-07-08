using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
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
            mvcBuilder.AddFluentValidation( fv=> 
                fv.RegisterValidatorsFromAssembly(Assembly.GetAssembly(typeof(ValidationExtensions))));
        }
    }
}
