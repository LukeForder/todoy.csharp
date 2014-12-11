using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Todoy.Features.Users.Dto
{
    public class RegistrationRequest
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

        public string PasswordConfirmation
        {
            get;
            set;
        }
    }
}
