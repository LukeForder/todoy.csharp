using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Todoy.Features.Todos.Models;

namespace Todoy.Features.Todos
{
    public interface IToDoStore
    {
        Task<ToDo> AddAsync(ToDo newToDo);

        Task<IEnumerable<ToDo>> GetAllByEmailAsync(string emailAddress);
    }
}
