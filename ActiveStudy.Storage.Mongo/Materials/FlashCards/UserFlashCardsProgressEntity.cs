using System.Collections.Generic;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace ActiveStudy.Storage.Mongo.Materials.FlashCards;

public class UserFlashCardsProgressEntity
{
    public ObjectId Id { get; set; }

    [BsonElement("userId")]
    public string UserId { get; set; }

    [BsonElement("progress")]
    public IDictionary<string, int> Progress { get; set; }
}