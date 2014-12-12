using MongoDB.Driver;
using Nancy.Bootstrappers.Ninject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Todoy.Features.Users.Data;
using Todoy.Features.Users.Data.Indexing;
using Todoy.Features.Users.Models;
using Ninject;
using FluentValidation;
using Todoy.Features.Users.Validators;
using Todoy.Features.Users.Impl;
using Todoy.Features.Users;
using Todoy.Features.Users.Dto;

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
                    var client = new MongoClient();

                    var server = client.GetServer();

                    var database = server.GetDatabase("todoy_uat");

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
        }


    }
}
