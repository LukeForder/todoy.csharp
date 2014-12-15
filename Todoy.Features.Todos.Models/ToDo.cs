using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Todoy.Features.Todos.Models
{
    public class ToDo
    {

        public ToDo()
        {
            Id = Guid.NewGuid();
        }

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

        public string CreatedBy
        {
            get;
            set;
        }
    }
}
