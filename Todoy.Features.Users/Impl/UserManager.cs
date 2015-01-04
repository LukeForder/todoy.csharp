using FluentValidation;
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
            _registrationRequestValidator = registrationRequestValidator; 
        }

        public async Task<User> RegisterUserAsync(Dto.RegistrationRequest registrationRequest)
        {
            // enforce valid registrations
             
            var validationResult = _registrationRequestValidator.Validate(registrationRequest);
            if (!validationResult.IsValid)
            {
                throw new ValidationException(validationResult.Errors);
            }

            string salt = PasswordOperations.GenerateSalt();

            User user = new User
            {
                EmailAddress = registrationRequest.EmailAddress,
                Salt = salt,
                PasswordHash = PasswordOperations.GenerateHash(registrationRequest.Password, salt),
                CreatedDate = DateTime.UtcNow,
                Verified = false,
                VerificationToken = Guid.NewGuid()
            };

            User persistedUser = _userStore.Add(user);

            // TODO: queue email to user with verification link

            return await Task.FromResult(persistedUser);
        }

        public  async Task<User> ValidateCredentialsAsync(LoginCredentialWithIPAddress credentials)
        {
            User user = _userStore.Get(credentials.EmailAddress);

            if (user == null)
            {
                return null;
            }

            string hashedAttempt = PasswordOperations.GenerateHash(credentials.Password, user.Salt);

            bool loginSucceeded = (hashedAttempt == user.PasswordHash);

            LoginAttempt attemptRecord =
                new LoginAttempt
                {
                    AttemptedDate = DateTime.UtcNow,
                    IPAddress = null,
                    Succeeded = true
                };


            user.LoginAttempts.Add(attemptRecord);

            _userStore.Update(user);

            return await Task.FromResult((loginSucceeded) ? user : null);
        }

        public async Task VerifyEmailAddressAsync(string emailAddress, Guid verificationToken)
        {
            User user = _userStore.Get(emailAddress);

            if (user == null)
            {
                return;
            }

            if (verificationToken == user.VerificationToken)
            {
                user.Verified = true;

                _userStore.Update(user);
            }
        }
    }
}
