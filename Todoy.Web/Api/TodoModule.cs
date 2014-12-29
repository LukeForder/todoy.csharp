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

namespace Todoy.Web.Api
{
    public class TodoModule : NancyModule
    {
        private readonly ITodoManager _todoManager;

        public TodoModule(ITodoManager todoManager)
        {
            this.RequiresAuthentication();

            _todoManager = todoManager;

            Post["api/todo", true] = (args, ct) => OnCreateTodoAsync();

            Get["api/todo", true] = (args, ct) => OnGetAllTodosAsync();

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

        private async Task<dynamic> OnGetAllTodosAsync()
        {
            try
            {
                var currentUserName = Context.CurrentUser.UserName;

                return await _todoManager.GetAllUsersTodosAsync(currentUserName);
            }
            catch (Exception exception)
            {
                // TODO: log the exception details

                return CreateErrorResponse("Something has gone horribly wrong :(");
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
                return
                    Negotiate
                    .WithStatusCode(HttpStatusCode.BadRequest)
                    .WithModel(
                        new Dto.ErrorDto
                        {
                            Errors = exception.Errors.Select(x => x.ErrorMessage)
                        });
            }
            catch (Exception exception)
            {
                //TODO: log and sanitize
                return CreateErrorResponse("Something has gone horribly wrong :(");
            }
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

    }
}