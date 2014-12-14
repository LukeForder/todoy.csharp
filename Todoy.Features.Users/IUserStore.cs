using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Todoy.Features.Users.Models;

namespace Todoy.Features.Users
{
    public interface IUserStore 
    {
        User Add(User user);

        User Get(string emailAddress);

        User Update(User user);

    }
}
