using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Todoy.Features.Users.Models;

namespace Todoy.Features.Users
{
    public interface IUserManager
    {
        Task<User> RegisterUserAsync(Dto.RegistrationRequest registrationRequest);

        Task<User> ValidateCredentialsAsync(LoginCredentialWithIPAddress credentials);
    }
}
