using FluentValidation;
using FluentValidation.Results;
using MongoDB.Driver;
using MongoDB.Driver.Builders;
using System;
using System.Collections.Generic;
using System.Diagnostics;
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

            try
            {
                var result = _userCollection.Insert(user);

                return user;
            }
            catch(MongoDuplicateKeyException duplicateKeyException)
            {
                // TODO: get the duplicate key property ;

                // HACK: shortcut
                throw new DuplicateEmailAddressException(user.EmailAddress);
            }
        }

        public User Get(string emailAddress)
        {
            IMongoQuery query = Query<User>.EQ(x => x.EmailAddress, emailAddress);

            User user = _userCollection.FindOne(query);

            return user;
        }


        public User Update(User user)
        {
             _userValidator.ValidateAndThrow(user);

             _userCollection.Save(user);

             return user;
        }
    }
}
