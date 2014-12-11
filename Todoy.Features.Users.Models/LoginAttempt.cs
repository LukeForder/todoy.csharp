using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Todoy.Features.Users.Models
{
    public  class LoginAttempt
    {
        public DateTime AttemptedDate
        {
            get;
            set;
        }

        public string IPAddress
        {
            get;
            set;
        }

        public bool Succeeded
        {
            get;
            set;
        }
    }
}
