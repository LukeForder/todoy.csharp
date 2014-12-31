using Moq;
using Nancy;
using Nancy.Authentication.Token;
using Nancy.Responses.Negotiation;
using Nancy.Security;
using Nancy.Testing;
using NSpec;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Todoy.Features.Users;
using Todoy.Features.Users.Dto;
using Todoy.Features.Users.Models;
using Todoy.Web.Api;

namespace Todoy.Web.Test
{
    public class user_api_spec : nspec
    {
        public user_api_spec()
        {
            Debugger.Launch();
        }

        static readonly string userName = "user-api-spec";

        Mock<IUserManager> userManager = null;

        Mock<ITokenizer> tokenizer = null;

        Browser browser = null;

        void before_each()
        {
            userManager = new Mock<IUserManager>();

            tokenizer = new Mock<ITokenizer>();

            var bootstrapper = 
                new ConfigurableBootstrapper(
                    config => 
                        config
                        .Module<UserModule>()
                        .Dependency<ITokenizer>(tokenizer.Object)
                        .Dependency<IUserManager>(userManager.Object));

            browser = 
                new Browser(
                    bootstrapper, 
                    ctx => ctx.Accept(new MediaRange("application/json")));
        }

        void when_registering_a_user()
        {
            xit["should return OK and successful registeration"] = todo;

            xit["should return a list of details as to why registeration failed on a bad request"] = todo;

            xit["should return a generic error message when an unexpected error occurs"] = todo;
        }

        void when_logging_in()
        {
            xit["should not allow users with unverified email addresses to login"] = todo;

            it["should return users api token should be on successfully login"] =
                () => {

                    var loginCredentials =
                        new LoginCredentials
                        {
                            EmailAddress = userName,
                            Password = "secret1"
                        };

                    string token = "secret-token";

                    User user = new User
                    {
                        EmailAddress = userName, // the minimum details need to create a token
                        Verified = true // user has verified their email address
                    };

                    userManager
                        .Setup(x => x.ValidateCredentialsAsync(It.IsAny<LoginCredentialWithIPAddress>()))
                        .Returns(Task.FromResult(user));

                    tokenizer
                        .Setup(x => x.Tokenize(It.IsAny<IUserIdentity>(), It.IsAny<NancyContext>()))
                        .Returns(token);

                    var response = browser.Post("api/login", ctx => ctx.JsonBody(loginCredentials));

                    response.StatusCode.should_be(HttpStatusCode.OK);

                    response.Body.AsString().should_be(token);

                };

            xit["should return a failure message on bad credentials"] = todo;

            xit["should return a generic error message when an unexpected error occurs"] =
                () => {
                    

                };
        }


    }
}
