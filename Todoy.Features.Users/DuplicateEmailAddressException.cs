using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Todoy.Features.Users
{
    public class DuplicateEmailAddressException : Exception
    {
        public DuplicateEmailAddressException(string emailAddress)
            : base()
        {
            EmailAddress = emailAddress;
        }

        public DuplicateEmailAddressException(string emailAddress, string message)
            : base(message)
        {
            EmailAddress = emailAddress;
        }

        public string EmailAddress
        {
            get;
            private set;
        }
    }
}
