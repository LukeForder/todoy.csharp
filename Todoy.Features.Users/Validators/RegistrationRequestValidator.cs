using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Todoy.Features.Users.Dto;

namespace Todoy.Features.Users.Validators
{
    public class RegistrationRequestValidator : AbstractValidator<RegistrationRequest>
    {
        public RegistrationRequestValidator()
        {
            RuleFor(x => x.EmailAddress)
                .NotEmpty()
                .EmailAddress();

            RuleFor(x => x.PasswordConfirmation)
                .NotEmpty();

            RuleFor(x => x.Password)
                .NotEmpty()
                .Equal(x => x.PasswordConfirmation)
                .WithMessage("'Password' should match confirmation.")
                .Length(10, 128)
                .Must((string password) => password.Any(c => char.IsDigit(c)))
                .WithMessage("'Password' must contain a number.")
                .Must((string password) => password.Any(c => char.IsUpper(c)))
                .WithMessage("'Password' must contain an uppercase character.")
                .Must((string password) => password.Any(c => char.IsLower(c)))
                .WithMessage("'Password' must contain a lowercase character.")
                .Must((string password) => password.Any(c => char.IsPunctuation(c) || char.IsWhiteSpace(c)))
                .WithMessage("'Password' must contain a punctation or space character.");
        }
    }
}
