using Nancy;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Nancy.Security;
using Todoy.Features.Todos;
using System.Threading.Tasks;
using Todoy.Features.Todos.Dto;
using Nancy.ModelBinding;
using Todoy.Features.Todos.Models;
using FluentValidation;
using Common.Logging;
using Newtonsoft.Json;

namespace Todoy.Web.Api
{
    public class TodoModule : NancyModule
    {
        readonly ILog _log = LogManager.GetCurrentClassLogger();

        private readonly ITodoManager _todoManager;

        public TodoModule(ITodoManager todoManager)
        {
            this.RequiresAuthentication();

            _todoManager = todoManager;

            Post["api/todo", true] = (args, ct) => OnCreateTodoAsync();

            Get["api/todo", true] = (args, ct) => OnGetAllTodosAsync();

            Patch["api/todo/{id:guid}/completed", true] = (args, ct) => OnCompleteTodoAsync((Guid)args.id);
        }

        private async Task<dynamic> OnCompleteTodoAsync(Guid id)
        {
            try 
	        {
                await _todoManager.CompleteTodoAsync(id);

                return HttpStatusCode.OK;
	        }
            catch (ValidationException exception)
            {
                return CreateValidationFailedResponse(exception);
            }
	        catch (Exception exception)
	        {
                LogError(exception);

                return CreateErrorResponse("An unexpected error prevent us from marking your todo as complete. :(");
	        }
        }


        private async Task<dynamic> OnGetAllTodosAsync()
        {
            try
            {
                var currentUserName = Context.CurrentUser.UserName;

                return await _todoManager.GetAllUsersTodosAsync(currentUserName);
            }
            catch (Exception exception)
            {
                LogError(exception);

                return CreateErrorResponse("Something has gone horribly wrong whilst fetching your todos. :(");
            }
        }

        private async Task<dynamic> OnCreateTodoAsync()
        {
            try
            {
                NewTodoDto dto = this.Bind<NewTodoDto>();

                CreateToDoCommand command = new CreateToDoCommand
                {
                    CreatedDate = DateTime.UtcNow,
                    CreatedBy = Context.CurrentUser.UserName,
                    Details = dto.Details
                };

                ToDo todo = await _todoManager.CreateAsync(command);

                return new ToDoDto
                {
                    Id = todo.Id,
                    DoneDate = todo.CreatedDate,
                    CreatedDate = todo.CreatedDate,
                    Details = todo.Details
                };
            }
            catch (ValidationException exception)
            {
                return CreateValidationFailedResponse(exception);                  
            }
            catch (Exception exception)
            {
                LogError(exception);

                return CreateErrorResponse("Something has gone horribly wrong creating your todo. :(");
            }
        }

        private dynamic CreateValidationFailedResponse(ValidationException exception)
        {
            return
                  Negotiate
                    .WithStatusCode(HttpStatusCode.BadRequest)
                    .WithModel(
                        new Dto.ErrorDto
                        {
                            Errors = exception.Errors.Select(x => x.ErrorMessage)
                        });
        }

        private dynamic CreateErrorResponse(params string[] messages)
        {
            return
                Negotiate
                .WithStatusCode(HttpStatusCode.InternalServerError)
                .WithModel(
                    new Dto.ErrorDto
                    {
                        Errors = messages
                    });
        }

        private void LogError(Exception exception)
        {
            object loggableException = exception.AsLoggable();

            string exceptionString = JsonConvert.SerializeObject(loggableException);

            _log.Error(exceptionString);
        }

    }
}