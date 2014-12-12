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

namespace Todoy.Web.Api
{
    public class UserModule : NancyModule
    {
        public UserModule(
            IUserManager userManager)
        {
            ILog log = LogManager.GetCurrentClassLogger();

            Post["api/register", true] =
                async (args, ct) =>
                {
                    try
                    {
                        var registrationRequest = this.Bind<RegistrationRequest>();

                        if (registrationRequest == null)
                        {
                            return CreateBadResponse("Your registration details contained bad data :?");
                        }

                        User user = await userManager.RegisterUserAsync(registrationRequest);

                        return user;
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
                };
        }
       
        private dynamic CreateBadResponse(string message)
        {
            return
                Negotiate
                    .WithStatusCode(HttpStatusCode.BadRequest)
                    .WithModel(
                        new Dto.ErrorDto
                        {
                            Errors = new string[] { message }
                        });
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