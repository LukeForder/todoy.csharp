using FluentValidation;
using MongoDB.Driver;
using Moq;
using NSpec;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Todoy.Features.Users.Data.Indexing;
using Todoy.Features.Users.Models;
using Todoy.Features.Users.Validators;

namespace Todoy.Features.Users.Data.Tests
{
    public class UserStoreSpec : nspec
    {
        MongoClient client = null;
        MongoServer server = null;
        MongoDatabase database = null;
        MongoCollection<User> usersCollection = null;
        UserValidator userValidator = null;
        UserStore userStore  = null;

        void before_each()
        {
            client = new MongoClient();

            server = client.GetServer();

            database = server.GetDatabase("todoy_test");

            usersCollection = database.GetCollection<User>("users");

            usersCollection.RemoveAll();

            userValidator = new UserValidator();

            userStore = new UserStore(usersCollection, userValidator);

            new UserIndexes()
                .CreateIndexes(usersCollection);
        }

        void it_should_save_a_user()
        {
            User user = new User
            {
                CreatedDate = DateTime.UtcNow,
                EmailAddress = "l@f.com",
                Id = Guid.NewGuid(),
                LoginAttempts = new List<LoginAttempt>(),
                PasswordHash = "aaaaaa",
                Salt = "aaaaaa",
                Verified = false
            };

            userStore.Add(user);

            User persistedUser = usersCollection.FindOneById(user.Id);

            persistedUser.should_not_be_null();

            usersCollection.RemoveAll();
        }


        void it_should_not_save_the_same_email_address_twice()
        {
            User user = new User
            {
                CreatedDate = DateTime.UtcNow,
                EmailAddress = "l@f.com",
                Id = Guid.NewGuid(),
                LoginAttempts = new List<LoginAttempt>(),
                PasswordHash = "aaaaaa",
                Salt = "aaaaaa",
                Verified = false
            };

            userStore.Add(user);

            User persistedUser = usersCollection.FindOneById(user.Id);

            persistedUser.should_not_be_null();

            // change the id to make the user a new user
            user.Id = Guid.NewGuid();

            expect<DuplicateEmailAddressException>(() => userStore.Add(user));
            
        }
    }
}
