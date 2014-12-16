using Nancy;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Nancy.Security;

namespace Todoy.Web.Api
{
    public class TodoModule : NancyModule
    {
        public TodoModule()
        {
            this.RequiresAuthentication();
           
            Post["api/todo", true] =
                async (args, ct) =>
                {


                    return HttpStatusCode.OK;
                };

            Get["api/todo", true] =
                async (args, ct) =>
                {
                    return HttpStatusCode.OK;
                };

            Put["api/todo/{id:guid}/completed", true] =
               async (args, ct) =>
               {
                   return HttpStatusCode.OK;
               };

            Put["api/todo/{id:guid}", true] =
               async (args, ct) =>
               {
                   return HttpStatusCode.OK;
               };

            Delete["api/todo/{id:guid}", true] =
               async (args, ct) =>
               {
                   return HttpStatusCode.OK;
               };
        }
    }
}