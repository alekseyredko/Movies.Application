using FluentValidation;
using Movies.Application.Models;

namespace Movies.Application.Validators.User
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
