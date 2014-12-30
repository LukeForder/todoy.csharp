using Moq;
using Nancy;
using Nancy.Responses.Negotiation;
using Nancy.Security;
using Nancy.Testing;
using Newtonsoft.Json;
using NSpec;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Todoy.Features.Todos;
using Todoy.Features.Todos.Dto;
using Todoy.Features.Todos.Models;
using Todoy.Web.Api;
using Todoy.Web.Infrastructure;

namespace Todoy.Web.Test
{
    public class todo_api_spec : nspec
    {
        Browser browser = null;

        Mock<ITodoManager> todoManager = null;

        Mock<IUserIdentity> userIdentity = null;

        void before_each()
        {
            userIdentity = new Mock<IUserIdentity>();

            userIdentity
                .Setup(x => x.UserName)
                .Returns("todo-api-spec");

            userIdentity.Setup(x => x.Claims).Returns(new string[0]);

            todoManager = new Mock<ITodoManager>();
    
            var bootstrapper = 
                new ConfigurableBootstrapper(
                    with => 
                       with
                       .RequestStartup((container, pipelines, context) => 
                           {
                               context.CurrentUser = userIdentity.Object;
                           })
                       .Module<TodoModule>()
                       .Dependencies<ITodoManager>(todoManager.Object));

            browser = new Browser(bootstrapper, config => config.Accept(new MediaRange("application/json")));

        }

        void when_completing_a_todo()
        {
            it["should return the created todo on successfully"] = 
                () => {
                    System.Diagnostics.Debugger.Launch();

                    ToDo createdToDo = new ToDo
                    {
                        Id = Guid.NewGuid(),
                        CreatedBy = "test",
                        CreatedDate = DateTime.UtcNow,
                        Details = "Test"
                    };

                    todoManager
                        .Setup(x => x.CreateAsync(It.IsAny<CreateToDoCommand>()))
                        .Returns(Task.FromResult(createdToDo));

                    NewTodoDto dto = new NewTodoDto
                    {
                        Details = "Test"
                    };

                    var response = browser.Post("/api/todo", config => config.JsonBody(dto));

                    response.StatusCode.should_be(HttpStatusCode.OK);
                    var todo = JsonConvert.DeserializeObject(response.Body.AsString());
                };
        }

    }
}
