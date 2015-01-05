using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Todoy.Features.Todos.Models;

namespace Todoy.Features.Todos.Validators
{
    public class ToDoValidator : AbstractValidator<ToDo>
    {
        public ToDoValidator()
        {
            RuleFor(x => x.CreatedDate)
                .NotEmpty();

            RuleFor(x => x.CreatedBy)
                .NotEmpty();

            RuleFor(x => x.Details)
                .NotEmpty();
        }
    }
}
