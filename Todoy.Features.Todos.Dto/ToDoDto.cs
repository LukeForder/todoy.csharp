using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Todoy.Features.Todos.Dto
{
    public class ToDoDto
    {
        public Guid Id
        {
            get;
            set;
        }

        public string Details
        {
            get;
            set;
        }

        public DateTime CreatedDate
        {
            get;
            set;
        }

        public DateTime? DoneDate
        {
            get;
            set;
        }

    }
}
