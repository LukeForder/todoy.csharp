using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Todoy.Features.Users.Models
{
    public class LoginCredentialWithIPAddress
    {
        public string EmailAddress
        {
            get;
            set;
        }

        public string Password
        {
            get;
            set;
        }

        public string IPAddress
        {
            get;
            set;
        }
    }
}
