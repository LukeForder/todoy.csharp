using Nancy;
using SquishIt.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using SquishIt.Less;
using System.IO;
using Todoy.Features.Users;

using Nancy.Security;

namespace Todoy.Web.Site
{
    public class SiteModule : NancyModule
    {
        private readonly IUserManager _userManager;

        public SiteModule(IUserManager userManager)
        {
            this.RequiresHttps();

            _userManager = userManager;


            Get["/"] = (args) =>
            {
                return View["index.html"];
            };

            Get["js/{name}", true] =
                async (args, ct) =>
                {
                    return
                        await CreateResponse(
                            Bundle
                            .JavaScript()
                            .RenderCached((string)args.name),
                            "application/javascript");

                };

            Get["css/{name}", true] =
                async (args, ct) =>
                {
                    return
                        await CreateResponse(
                            Bundle
                            .Css()
                            .RenderCached((string)args.name),
                            "text/css");
                };

            Get["verify/{token}", true] = (args, ct) => OnVerifyEmailAddress(args.token);

        }

        private async Task<dynamic> OnVerifyEmailAddress(string token)
        {
            byte[] tokenBytes = Convert.FromBase64String(token);

            string verificationDetails = Encoding.UTF8.GetString(tokenBytes);

            string[] parts = verificationDetails.Split(';');

            Guid verificationGuid;

            if (parts.Length == 2 && Guid.TryParse(parts[1], out verificationGuid))
            {
                await _userManager.VerifyEmailAddressAsync(parts[0], verificationGuid);
            }

            return Response.AsRedirect("/");
        }


        private async Task<Response> CreateResponse(string content, string mimeType)
        {
            MemoryStream stream = new MemoryStream();

            using (StreamWriter writer = new StreamWriter(stream, Encoding.UTF8, 1024, true))
            {
                writer.AutoFlush = true;

                await writer.WriteAsync(content);

                stream.Position = 0;
            }

            return 
                Response
                .FromStream(stream, mimeType);
        }
    }
}
