using Nancy;
using SquishIt.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using SquishIt.Less;
using System.IO;

namespace Todoy.Web.Site
{
    public class SiteModule : NancyModule
    {
        public SiteModule()
        {
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
