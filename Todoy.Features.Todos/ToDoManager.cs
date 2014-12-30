using FluentValidation;
using FluentValidation.Results;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Todoy.Features.Todos.Models;

namespace Todoy.Features.Todos
{
    public class TodoManager : ITodoManager
    {
        private readonly IToDoStore _toDoStore;
        private readonly IValidator<ToDo> _toDoValidator;

        public TodoManager(
            IToDoStore toDoStore,
            IValidator<ToDo> toDoValidator)
        {
            _toDoStore = toDoStore;
            _toDoValidator = toDoValidator;
        }

        public async Task<Models.ToDo> CreateAsync(Models.CreateToDoCommand createCommand)
        {
            ToDo toDo = new ToDo
            {
                Id = Guid.NewGuid(),
                Details = createCommand.Details,
                DoneDate = createCommand.CompletedDate,
                CreatedBy = createCommand.CreatedBy,
                CreatedDate = createCommand.CreatedDate
            };

            _toDoValidator.ValidateAndThrow(toDo);
           
            return await _toDoStore.AddAsync(toDo);            
        }

        public async Task<IEnumerable<ToDo>> GetAllUsersTodosAsync(string emailAddress)
        {
            return await _toDoStore.GetAllByEmailAsync(emailAddress);
        }

        public async Task<ToDo> GetAsync(Guid id)
        {
            return await _toDoStore.GetAsync(id);
        }

        public async Task CompleteTodoAsync(Guid id)
        {
            ToDo todo = await this.GetAsync(id);
            if (todo != null)
            {
                todo.DoneDate = DateTime.UtcNow;

                await _toDoStore.SaveAsync(todo);
            }
        }
    }
}
