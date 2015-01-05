using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Todoy.Features.Todos.Models
{
    public class CompleteToDoCommand
    {
        public string UserName
        {
            get;
            set;
        }

        public Guid ToDoId
        {
            get;
            set;
        }
    }
}
