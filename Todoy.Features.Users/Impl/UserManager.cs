﻿using FluentValidation;
using FluentValidation.Results;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Todoy.Features.Users.Dto;
using Todoy.Features.Users.Models;

namespace Todoy.Features.Users.Impl
{
    public class UserManager : IUserManager
    {
        private readonly IUserStore _userStore;
        private IValidator<RegistrationRequest> _registrationRequestValidator;
        private IValidator<User> _userValidator;

        public UserManager(
            IUserStore userStore,
            IValidator<User> userValidator,
            IValidator<RegistrationRequest> registrationRequestValidator)
        {
            _userStore = userStore;
            _userValidator = userValidator;
        }

        public async Task<User> RegisterUserAsync(Dto.RegistrationRequest registrationRequest)
        {
            // enforce valid registrations
            _registrationRequestValidator.ValidateAndThrow(registrationRequest);

            string salt = PasswordOperations.GenerateSalt();

            User user = new User
            {
                EmailAddress = registrationRequest.EmailAddress,
                Salt = salt,
                PasswordHash = PasswordOperations.GenerateHash(registrationRequest.Password, salt),
                CreatedDate = DateTime.UtcNow,
                Verified = false
            };

            User persistedUser = _userStore.Add(user);

            return persistedUser;
        }
    }
}
