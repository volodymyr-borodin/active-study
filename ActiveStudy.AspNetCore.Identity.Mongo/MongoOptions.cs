using MongoDB.Driver;

namespace ActiveStudy.AspNetCore.Identity.Mongo
{
    public class MongoOptions
    {
        public MongoClientSettings ClientSettings { get; }

        public string DbName { get; }

        public MongoOptions(MongoClientSettings clientSettings, string dbName)
        {
            ClientSettings = clientSettings;
            DbName = dbName;
        }
    }
}