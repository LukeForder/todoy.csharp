using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Todoy.Features.Todos.Models;

namespace Todoy.Features.Todos
{
    public class ToDoManager : ITodoManager
    {
        public Task<Models.ToDo> CreateAsync(Models.CreateToDoCommand createCommand)
        {
            return null;
        }
    }
}
