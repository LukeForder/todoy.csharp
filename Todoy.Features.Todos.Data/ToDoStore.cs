using FluentValidation;
using MongoDB.Driver;
using MongoDB.Driver.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Todoy.Features.Todos.Models;

namespace Todoy.Features.Todos.Data
{
    public class TodoStore : IToDoStore
    {
        private readonly MongoCollection<ToDo> _toDoCollection;
        private readonly IValidator<ToDo> _toDoValidator;

        public TodoStore(
            MongoCollection<ToDo> toDoCollection,
            IValidator<ToDo> toDoValidator)
        {
            _toDoCollection = toDoCollection;
            _toDoValidator = toDoValidator;
        }

        public async Task<Models.ToDo> AddAsync(Models.ToDo newToDo)
        {
            _toDoValidator.ValidateAndThrow(newToDo);

            _toDoCollection.Insert(newToDo);

            // only sync operations supported, spoof the async.
            return await Task.FromResult(newToDo);
        }

        public async Task<IEnumerable<ToDo>> GetAllByEmailAsync(string emailAddress)
        {
            IMongoQuery query = Query<ToDo>.EQ(x => x.CreatedBy, emailAddress);

            var results = 
                _toDoCollection
                .Find(query)
                .ToList();

            return await Task.FromResult(results);
        }
    }
}
