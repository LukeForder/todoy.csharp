using MongoDB.Driver;
using MongoDB.Driver.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Todoy.Features.Users.Models;

namespace Todoy.Features.Users.Data.Indexing
{
    public class UserIndexes
    {
        public void CreateIndexes(MongoCollection<User> userCollection)
        {
            userCollection
                .CreateIndex(
                    new IndexKeysBuilder()
                        .Ascending("EmailAddress"),
                    IndexOptions.SetUnique(true));
        }
    }
}
