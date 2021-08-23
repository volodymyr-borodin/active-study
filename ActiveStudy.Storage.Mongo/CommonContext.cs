using System;
using ActiveStudy.Storage.Mongo.Crm;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Driver;

namespace ActiveStudy.Storage.Mongo
{
    public class CommonContext
    {
        private readonly IMongoDatabase database;
        public IMongoCollection<AuditItemEntity> Audit => database.GetCollection<AuditItemEntity>("audit");

        public CommonContext(MongoUrl url)
        {
            var client = new MongoClient(MongoClientSettings.FromUrl(url));
            database = client.GetDatabase(url.DatabaseName);
        }
    }

    public class AuditItemEntity
    {
        public ObjectId Id { get; set; }

        [BsonElement("actionId")]
        public Guid ActionId { get; set; }

        [BsonElement("time")]
        public DateTime Time { get; set; }

        [BsonElement("user")]
        public UserEntity User { get; set; }

        [BsonElement("data")]
        public BsonDocument Data { get; set; }
    }
}