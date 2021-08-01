using ActiveStudy.AspNetCore.Identity.Mongo;
using MongoDB.Driver;

namespace ActiveStudy.Storage.Mongo.Identity
{
    public class IdentityContext : IdentityDbContext<string>
    {
        public IdentityContext(IMongoDatabase database) : base(database)
        { }
    }
}