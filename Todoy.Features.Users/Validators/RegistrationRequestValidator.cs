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
                .WithMessage("'Password' should match confirmation.");



        }
    }
}
