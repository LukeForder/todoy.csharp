using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Todoy.Features.Users.Models;

namespace Todoy.Features.Users.Validators
{
    public class UserValidator : AbstractValidator<User>
    {
        public UserValidator()
        {
            RuleFor(x => x.Id)
                .NotEmpty();

            RuleFor(x => x.EmailAddress)
                .NotEmpty()
                .EmailAddress();

            RuleFor(x => x.PasswordHash)
                .NotEmpty();

            RuleFor(x => x.Salt)
                .NotEmpty();

            RuleFor(x => x.CreatedDate)
                .NotEmpty();

            RuleFor(x => x.LoginAttempts)
                .NotNull();
        }
    }
}
