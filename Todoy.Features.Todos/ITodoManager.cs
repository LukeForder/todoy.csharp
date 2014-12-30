using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Todoy.Features.Todos.Models;

namespace Todoy.Features.Todos
{
    public interface ITodoManager
    {
        Task<ToDo> CreateAsync(CreateToDoCommand createCommand);

        Task<IEnumerable<ToDo>> GetAllUsersTodosAsync(string emailAddress);

        Task<ToDo> GetAsync(Guid id);

        Task CompleteTodoAsync(Guid id);
    }
}
