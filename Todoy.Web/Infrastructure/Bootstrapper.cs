using MongoDB.Driver;
using Nancy.Bootstrappers.Ninject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Todoy.Features.Users.Data;
using Todoy.Features.Users.Models;
using Ninject;
using FluentValidation;
using Todoy.Features.Users.Validators;
using Todoy.Features.Users.Impl;
using Todoy.Features.Users;
using Todoy.Features.Users.Dto;
using Todoy.Features.Users.Data.Indexing;
using Nancy.Authentication.Token;
using SquishIt.Framework;
using Nancy.Conventions;
using Todoy.Features.Todos;
using Todoy.Features.Todos.Data;
using Todoy.Features.Todos.Models;
using Todoy.Features.Todos.Validators;
using System.Configuration;

namespace Todoy.Web.Infrastructure
{
    public class Bootstrapper : NinjectNancyBootstrapper
    {
        protected override void ConfigureApplicationContainer(Ninject.IKernel existingContainer)
        {
            base.ConfigureApplicationContainer(existingContainer);

            existingContainer
                .Bind<MongoDatabase>()
                .ToMethod((ctx) =>
                {
                    string mongoDBConnectionString = ConfigurationManager.AppSettings["mongolab-uri"];

                    string todoyDBName = ConfigurationManager.AppSettings["database-name"];

                    var client = new MongoClient(mongoDBConnectionString);

                    var server = client.GetServer();

                    var database = server.GetDatabase(todoyDBName);

                    return database;
                });


        }

        protected override void ConfigureRequestContainer(IKernel container, Nancy.NancyContext context)
        {
            base.ConfigureRequestContainer(container, context);

            container
                .Bind<MongoCollection<User>>()
                .ToMethod(
                    (ctx) =>
                    {
                        var database = ctx.Kernel.Get<MongoDatabase>();

                        var usersCollection = database.GetCollection<User>("users");

                        new UserIndexes()
                            .CreateIndexes(usersCollection);

                        return usersCollection;
                    });

            container
              .Bind<MongoCollection<ToDo>>()
              .ToMethod(
                  (ctx) =>
                  {
                      var database = ctx.Kernel.Get<MongoDatabase>();

                      var todoCollection = database.GetCollection<ToDo>("todos");

                      return todoCollection;
                  });


            container
                .Bind<IValidator<User>>()
                .To<UserValidator>();

            container
                .Bind<IValidator<RegistrationRequest>>()
                .To<RegistrationRequestValidator>();

            container
                .Bind<IUserManager>()
                .To<UserManager>();

            container
                .Bind<IUserStore>()
                .To<UserStore>();

            container
                .Bind<IToDoStore>()
                .To<TodoStore>();

            container
                .Bind<IValidator<ToDo>>()
                .To<ToDoValidator>();

            container
                .Bind<ITodoManager>()
                .To<TodoManager>();

            container
                .Bind<ITokenizer>()
                .To<Tokenizer>();
        }

        protected override void RequestStartup(IKernel container, Nancy.Bootstrapper.IPipelines pipelines, Nancy.NancyContext context)
        {
            base.RequestStartup(container, pipelines, context);


            TokenAuthentication.Enable(pipelines, new TokenAuthenticationConfiguration(container.Get<ITokenizer>()));
        }

        protected override void ApplicationStartup(IKernel container, Nancy.Bootstrapper.IPipelines pipelines)
        {
            base.ApplicationStartup(container, pipelines);

            Bundle
                .JavaScript()
                .Add("~/Scripts/jquery-2.1.1.js")
                .Add("~/Scripts/angular.js")
                .Add("~/Scripts/angular-route.js")
                .Add("~/Scripts/angular-input-match.js")
                .Add("~/Scripts/angular-local-storage.js")
                .Add("~/Scripts/angular-cookies.js")
                .Add("~/Scripts/moment.js")
                .WithMinifier<SquishIt.Framework.Minifiers.JavaScript.MsMinifier>()
                .ForceRelease()
                .AsCached("all", "~/js/all");

            Bundle
                .Css()
                .Add("~/Content/font-awesome.css")
                .Add("~/Less/bootstrap.less")
                .ProcessImports()
                .Add("~/Less/bootswatch.less")
                .Add("~/Less/custom.less")
                .WithMinifier<SquishIt.Framework.Minifiers.CSS.YuiMinifier>()
                .ForceRelease()
                .AsCached("all", "~/css/all");

            Bundle
                .JavaScript()
                .AddDirectory("~/JsApp/authentication")
                .AddDirectory("~/JsApp/todo")
                .Add("~/JsApp/module.js")
                .ForceRelease()
                .AsCached("app", "~/js/app");

        }

        protected override void ConfigureConventions(Nancy.Conventions.NancyConventions nancyConventions)
        {
            base.ConfigureConventions(nancyConventions);

            nancyConventions
                .StaticContentsConventions
                .Add(StaticContentConventionBuilder.AddDirectory("Scripts"));

            nancyConventions
                .StaticContentsConventions
                .Add(StaticContentConventionBuilder.AddDirectory("Less"));

            nancyConventions
               .StaticContentsConventions
               .Add(StaticContentConventionBuilder.AddDirectory("fonts"));

            nancyConventions
                .StaticContentsConventions
                .Add(StaticContentConventionBuilder.AddDirectory("JsApp/authentication/partials"));

            nancyConventions
               .StaticContentsConventions
               .Add(StaticContentConventionBuilder.AddDirectory("JsApp/todo/partials"));

            nancyConventions
               .StaticContentsConventions
               .Add(StaticContentConventionBuilder.AddDirectory("fonts"));
        }
    }
}
