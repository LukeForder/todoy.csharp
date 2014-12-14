using Common.Logging;
using Nancy;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Nancy.ModelBinding;
using Todoy.Features.Users;
using Todoy.Features.Users.Dto;
using Todoy.Features.Users.Models;
using System.Threading.Tasks;
using Nancy.Authentication.Token;
using Todoy.Web.Infrastructure;

namespace Todoy.Web.Api
{
    public class UserModule : NancyModule
    {
        private readonly IUserManager _userManager;
        private readonly ITokenizer _tokenizer;

        public UserModule(
            IUserManager userManager,
            ITokenizer tokenizer)
        {
            ILog log = LogManager.GetCurrentClassLogger();

            _userManager = userManager;
            _tokenizer = tokenizer;

            Post["api/register", true] =
                async (args, ct) =>
                {
                    RegistrationRequest registrationRequest = null; 
                    
                    if (TryBind<RegistrationRequest>(out registrationRequest))
                    {
                        return await OnRegisterNewUser(registrationRequest);
                    }
                    else
                    {
                        return CreateBadResponse("Your registration details contained bad data :?");
                    }
                };

            Post["api/login", true] =
                async (args, ct) =>
                {
                    LoginCredentials loginCredentials = null;

                    if (TryBind<LoginCredentials>(out loginCredentials))
                    {
                        // Overwrite the IP Address
                        LoginCredentialWithIPAddress fullCredentials =
                            new LoginCredentialWithIPAddress
                            {
                                EmailAddress = loginCredentials.EmailAddress,
                                Password = loginCredentials.Password,
                                IPAddress = Request.UserHostAddress
                            };

                        return await OnLoginAttempt(fullCredentials);
                    }
                    else
                    {
                        return CreateBadResponse("Invalid email address or password combination.");
                    }

                };
        }

        private async Task<dynamic> OnLoginAttempt(LoginCredentialWithIPAddress loginCredentials)
        {

            User user = await _userManager.ValidateCredentialsAsync(loginCredentials);
            
            if (user == null)
            {
                return HttpStatusCode.Unauthorized;
            }

            var nancyUser = new UserIdentity(user.EmailAddress, new string[0]);

            var authenticationToken = _tokenizer.Tokenize(nancyUser, Context);


            return authenticationToken;
        }

        public async Task<dynamic> OnRegisterNewUser(RegistrationRequest registrationRequest)
        {
            try
            {
                User user = await _userManager.RegisterUserAsync(registrationRequest);

                return HttpStatusCode.OK;
            }
            catch (DuplicateEmailAddressException emailAddressException)
            {
                // TODO: log the email address for the duplicate?

                return CreateBadResponse("An account already exists with that email address");
            }
            catch (FluentValidation.ValidationException validationException)
            {
                return CreateBadResponse(
                    validationException
                    .Errors
                    .Select(x => x.ErrorMessage)
                    .ToArray());
            }
            catch (Nancy.ModelBinding.ModelBindingException exception)
            {
                // TODO: log the exception details

                return CreateBadResponse("Your registration details contained bad data :?");
            }
        }
       
        private bool TryBind<T>(out T boundRequestData)
        {
            boundRequestData = default(T);

            try
            {
                boundRequestData = this.Bind<T>();

                return true;
            }
            catch (Exception ex)
            {
                // TODO: log
            }

            return false;
        }

        private dynamic CreateBadResponse(string message)
        {
            return CreateBadResponse(new string[] { message });
        }

        private dynamic CreateBadResponse(IEnumerable<string> messages)
        {
            return
                Negotiate
                    .WithStatusCode(HttpStatusCode.BadRequest)
                    .WithModel(
                        new Dto.ErrorDto
                        {
                            Errors = messages 
                        });
        }
    }
}