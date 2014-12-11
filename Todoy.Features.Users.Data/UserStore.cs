using FluentValidation;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Todoy.Features.Users.Models;

namespace Todoy.Features.Users.Data
{
    public class UserStore : IUserStore
    {
        private MongoCollection<User> _userCollection;
        private IValidator<User> _userValidator;

        public UserStore(
            MongoCollection<User> userCollection,
            IValidator<User> userValidator)
        {
            _userCollection = userCollection;
            _userValidator = userValidator;
        }

        public User Add(User user)
        {
            _userValidator.ValidateAndThrow(user);

            var result = _userCollection.Insert(user);

            return user;
        }
    }
}
