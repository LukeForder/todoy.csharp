using FluentValidation;
using FluentValidation.Results;
using MongoDB.Driver;
using Moq;
using NSpec;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Todoy.Features.Todos.Data;
using Todoy.Features.Todos.Models;

namespace Todoy.Features.Todos.Tests
{
    public class todo_store_spec : nspec
    {

        MongoClient client = null;
        MongoServer server = null;
        MongoDatabase database = null;
        MongoCollection<ToDo> toDoCollection = null;
        Mock<IValidator<ToDo>> toDoValidator = null;
        IToDoStore toDoStore = null;

        void before_each()
        {
            client = new MongoClient();

            server = client.GetServer();

            database = server.GetDatabase("todoy_test");

            toDoCollection = database.GetCollection<ToDo>("todos");

            toDoCollection.RemoveAll();

            toDoValidator = new Mock<IValidator<ToDo>>();
        
            toDoStore = new TodoStore(toDoCollection, toDoValidator.Object);
        }

        void when_creating_a_todo()
        {
            it["creates validated todos"] =
                async () =>
                {

                    ToDo newToDo = new ToDo
                    {
                        Id = Guid.NewGuid(),
                        CreatedBy = "a@a.co.za",
                        CreatedDate = DateTime.Now,
                        Details = "Test Todo",
                        DoneDate = null
                    };

                    toDoValidator
                        .Setup(x => x.Validate(It.Is<ToDo>(m => m.Id == newToDo.Id)))
                        .Returns(new ValidationResult());

                    await toDoStore.AddAsync(newToDo);

                    toDoCollection.FindOneById(newToDo.Id).should_not_be_null();

                    toDoValidator.Verify(
                        x => x.Validate(It.Is<ToDo>(m => m.Id == newToDo.Id)),
                        Times.AtLeastOnce);
                };

            it["rejects invalid todos"] = todo;
              
        }

          
    }
}
