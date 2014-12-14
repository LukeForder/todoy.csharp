using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Todoy.Features.Users.Models
{
    public class User
    {
        public User()
        {
            Id = Guid.NewGuid();

            LoginAttempts = new List<LoginAttempt>();
        }

        public Guid Id
        {
            get;
            set;
        }

        public string EmailAddress
        {
            get;
            set;
        }

        public string PasswordHash
        {
            get;
            set;
        }

        public string Salt
        {
            get;
            set;
        }

        public DateTime CreatedDate
        {
            get;
            set;
        }

        public ICollection<LoginAttempt> LoginAttempts
        {
            get;
            set;
        }

        public bool Verified
        {
            get;
            set;
        }

    }
}
