using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Todoy.Features.Todos.Models
{
    public class CreateToDoCommand
    {
        public string CreatedBy
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

        public DateTime? CompletedDate
        {
            get;
            set;
        }
    }
}
