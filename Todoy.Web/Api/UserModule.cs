using Common.Logging;
using Nancy;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Nancy.ModelBinding;

namespace Todoy.Web.Api
{
    public class UserModule : NancyModule
    {
        public UserModule()
        {
            ILog log = LogManager.GetCurrentClassLogger();

            Post["api/register", true] =
                async (args, ct) =>
                {
                    //try
                    //{
                    //    Dto.RegisterDto registrationMessage = this.Bind<Dto.RegisterDto>();
                    //}
                    //catch  (Nancy.ModelBinding.ModelBindingException exception)
                    //{
                    //    // TODO: log the exception details

                    //    return
                    //        Negotiate
                    //        .WithStatusCode(HttpStatusCode.BadRequest)
                    //        .WithModel(
                    //            new Dto.ErrorDto
                    //            {
                    //                Errors = new string[] { "Your registration details contained bad data :?" }
                    //            });

                    //}

                    return null;
                    
                };
        }
    }
}