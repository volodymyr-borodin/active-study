using System;
using System.Collections.Generic;
using ActiveStudy.Domain;
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

        [BsonElement("message")]
        public string Message { get; set; }

        [BsonElement("time")]
        public DateTime Time { get; set; }

        [BsonElement("user")]
        public UserEntity User { get; set; }

        [BsonElement("entities")]
        public IEnumerable<AuditObjectEntity> Entities { get; set; }
    }

    public class AuditObjectEntity
    {
        [BsonElement("id")]
        public string Id { get; set; }

        [BsonElement("entityType")]
        public EntityType EntityType { get; set; }
    }
}