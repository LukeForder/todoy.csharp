using Nancy.Security;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Todoy.Web.Infrastructure
{
    public class UserIdentity : IUserIdentity
    {
        private string _userName;
        private string[] _claims;

        public UserIdentity(
            string userName,
            IEnumerable<string> claims)
        {
            _userName = userName;
            _claims = claims.ToArray();
        }

        public IEnumerable<string> Claims
        {
            get 
            {
                return _claims;
            }
        }

        public string UserName
        {
            get
            {
                return _userName;
            }
        }
    }
}