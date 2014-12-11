using NSpec;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Todoy.Features.Users.Validators;

namespace Todoy.Features.Users.Test
{
    public class RegistrationValidatorSpec : nspec
    {
        void it_rejects_requests_with_empty_passwords()
        {
            Dto.RegistrationRequest request = 
                new Dto.RegistrationRequest
                {
                    EmailAddress = "foo@bar.com",
                    Password = "",
                    PasswordConfirmation = ""
                };

            RegistrationRequestValidator requestValidator = new RegistrationRequestValidator();

            var validationResult = requestValidator.Validate(request);

            validationResult.IsValid.should_be(false);
            validationResult.Errors.Select(error => error.PropertyName).should_contain("Password");

        }

        void it_rejects_requests_with_passwords_and_non_matching_passwords()
        {
            Dto.RegistrationRequest request =
               new Dto.RegistrationRequest
               {
                   EmailAddress = "foo@bar.com",
                   Password = "eeee",
                   PasswordConfirmation = "aoeaoeao"
               };

            RegistrationRequestValidator requestValidator = new RegistrationRequestValidator();

            var validationResult = requestValidator.Validate(request);

            validationResult.IsValid.should_be(false);
            validationResult.Errors.Select(error => error.PropertyName).should_contain("Password");
        }

        void it_rejects_requests_with_invalid_email_addresses()
        {

            Dto.RegistrationRequest request =
               new Dto.RegistrationRequest
               {
                   EmailAddress = "f.com",
                   Password = "eeeeee",
                   PasswordConfirmation = "eeeeee"
               };

            RegistrationRequestValidator requestValidator = new RegistrationRequestValidator();

            var validationResult = requestValidator.Validate(request);

            validationResult.IsValid.should_be(false);
            validationResult.Errors.Select(error => error.PropertyName).should_contain("EmailAddress");

        }


        void it_approves_valid_requests()
        {

            Dto.RegistrationRequest request =
               new Dto.RegistrationRequest
               {
                   EmailAddress = "f@b.com",
                   Password = "eeeeee",
                   PasswordConfirmation = "eeeeee"
               };

            RegistrationRequestValidator requestValidator = new RegistrationRequestValidator();

            var validationResult = requestValidator.Validate(request);

            validationResult.IsValid.should_be(true);

        }
    }
}
