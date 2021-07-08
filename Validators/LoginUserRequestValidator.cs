using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Threading.Tasks;
using FluentValidation;
using Movies.Application.Models;

namespace Movies.Application.Validators
{
    public class LoginUserRequestValidator: AbstractValidator<LoginUserRequest>
    {
        public LoginUserRequestValidator()
        {
            RuleFor(x => x.Login).NotEmpty().Length(5, 25);
            RuleFor(x => x.Password).NotEmpty().Length(5, 25);
        }
    }
}
