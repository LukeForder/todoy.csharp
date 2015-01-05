using FluentValidation;
using FluentValidation.Results;
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
        // TODO: programmatically verify that exceptions are logged
        // TODO: test for authentication required

        Browser browser = null;

        Mock<ITodoManager> todoManager = null;

        Mock<IUserIdentity> userIdentity = null;

        static readonly string userName = "todo-api-spec";

        void before_each()
        {
            userIdentity = new Mock<IUserIdentity>();

            userIdentity
                .Setup(x => x.UserName)
                .Returns(userName);

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

        void when_creating_a_new_todo()
        {
            it["should return the created todo on successful creation"] = 
                () => {
                   
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

                    NewTodoDto dto = CreateToDo("Test");

                    var response = browser.Post("/api/todo", config => config.JsonBody(dto));

                    response.StatusCode.should_be(HttpStatusCode.OK);
                    var todo = JsonConvert.DeserializeObject(response.Body.AsString());
                };

            it["should returns a bad request detailing the reasons for failure when the new todo is invalid"] = () =>
                {
                    // The exception thrown the ToDoManager during the creation process.
                    List<ValidationFailure> failures = new 
                        List<ValidationFailure> 
                        {
                            new ValidationFailure("Details", "The todo must be given details.")
                        };

                    ValidationException exception = new ValidationException(failures);

                    todoManager
                        .Setup(x => x.CreateAsync(It.IsAny<CreateToDoCommand>()))
                        .Throws(exception);

                    NewTodoDto dto = CreateToDo(null);

                    var response = browser.Post("/api/todo", config => config.JsonBody(dto));

                    response.StatusCode.should_be(HttpStatusCode.BadRequest);

                    var errorResponse = JsonConvert.DeserializeObject<Dto.ErrorDto>(response.Body.AsString());

                    errorResponse.Errors.Count().should_be(failures.Count);
                    errorResponse.Errors.ElementAt(0).should_be(failures[0].ErrorMessage);
                };

            it["should return a generic error response on an unhandled internal error"] =
                () =>
                {

                    Exception unhandledException = new Exception("BOOM!");

                    todoManager
                      .Setup(x => x.CreateAsync(It.IsAny<CreateToDoCommand>()))
                      .Throws(unhandledException);

                    NewTodoDto dto = CreateToDo(null);

                    var response = browser.Post("/api/todo", config => config.JsonBody(dto));

                    AssertGenericResponseReturnedOnError(response, "Something has gone horribly wrong creating your todo. :(");

                };
        }

        void when_completing_a_todo()
        {
            it["returns an OK response on success"] =
                () => {

                    todoManager
                        .Setup(x => x.CompleteTodoAsync(It.IsAny<CompleteToDoCommand>()))
                        .Returns(Task.FromResult<object>(null));

                    var response = browser.Patch(string.Format("api/todo/{0}/completed", Guid.NewGuid()));

                    response.StatusCode.should_be(HttpStatusCode.OK);
                };

            it["should return a bad request detailing the reasons for failure when the completed todo is invalid"] = () => {

                var validationFailures = new ValidationFailure[] {
                    new ValidationFailure("Details", "Error!")
                };

                ValidationException exception = new ValidationException(validationFailures);

                todoManager
                    .Setup(x => x.CompleteTodoAsync(It.IsAny<CompleteToDoCommand>()))
                    .Throws(exception);

                var response = browser.Patch(string.Format("api/todo/{0}/completed", Guid.NewGuid()));

                response.StatusCode.should_be(HttpStatusCode.BadRequest);

                var errorDto =
                    JsonConvert.DeserializeObject<Dto.ErrorDto>(
                        response.Body.AsString());

                errorDto.Errors.should_not_be_empty();
                errorDto.Errors.ElementAt(0).should_be("Error!");
            };

            it["should return a generic error response on an unhandled internal error"] = () =>
                {
                    var unhandledException = new Exception("BOOM!");

                    todoManager
                        .Setup(x => x.CompleteTodoAsync(It.IsAny<CompleteToDoCommand>()))
                        .Throws(unhandledException);


                    var response = browser.Patch(string.Format("api/todo/{0}/completed", Guid.NewGuid()));

                    response.StatusCode.should_be(HttpStatusCode.InternalServerError);

                    var errorDto = 
                        JsonConvert.DeserializeObject<Dto.ErrorDto>(
                            response.Body.AsString());

                    errorDto
                        .Errors
                        .should_not_be_empty();

                    errorDto
                        .Errors
                        .ElementAt(0)
                        .should_be("An unexpected error prevent us from marking your todo as complete. :(");

                };
        }

        void when_getting_all_todos()
        {
            it["returns all the users todos on success"] = 
                () => {

                    IEnumerable<ToDo> usersTodos = new ToDo[] 
                    {
                        new ToDo
                        {
                            Id = Guid.NewGuid(),
                            Details = "Test1",
                            CreatedBy = userName,
                            CreatedDate = new DateTime(2014, 1, 1)
                        }
                    };

                    todoManager
                        .Setup(x => x.GetAllUsersTodosAsync(It.Is<string>(u => u == userName)))
                        .Returns(Task.FromResult(usersTodos));

                    var response = browser.Get("api/todo");

                    response.StatusCode.should_be(HttpStatusCode.OK);
                    
                    var todos = JsonConvert.DeserializeObject<IEnumerable<ToDo>>(response.Body.AsString());

                    todos.should_not_be_null();
                    todos.Count().should_be(1);
                };

            it["should return a generic error response on an unhandled internal error"] = 
                () => {
                    var unhandledException = new Exception("BOOM!");

                    todoManager
                        .Setup(x => x.GetAllUsersTodosAsync(It.IsAny<string>()))
                        .Throws(unhandledException);

                    var response = browser.Get("api/todo");

                    response.StatusCode.should_be(HttpStatusCode.InternalServerError);
                    var errorDto = JsonConvert.DeserializeObject<Dto.ErrorDto>(response.Body.AsString());

                    errorDto.Errors.should_not_be_empty();
                    errorDto.Errors.ElementAt(0).should_be("Something has gone horribly wrong whilst fetching your todos. :(");
                };
        }

        private static NewTodoDto CreateToDo(string details)
        {
            NewTodoDto dto = new NewTodoDto
            {
                Details = details
            };

            return dto;
        }

        private static void AssertGenericResponseReturnedOnError(BrowserResponse response, string expectedErrorMessage)
        {
            response.StatusCode.should_be(HttpStatusCode.InternalServerError);

            var errorResponse = JsonConvert.DeserializeObject<Dto.ErrorDto>(response.Body.AsString());

            errorResponse.Errors.Count().should_be(1);
            errorResponse.Errors.ElementAt(0).should_be(expectedErrorMessage);
        }

    }
}
