using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentValidation;
using Movies.Application.Models;

namespace Movies.Application.Validators
{
    public class RegisterUserValidator: AbstractValidator<RegisterUserRequest>
    {
        public RegisterUserValidator()
        {
            RuleFor(x => x.Login).NotEmpty().Length(5,25);
            RuleFor(x => x.Name).NotEmpty().Length(5,25);
            RuleFor(x => x.Password).NotEmpty().Length(8,25);
        }
    }
}
