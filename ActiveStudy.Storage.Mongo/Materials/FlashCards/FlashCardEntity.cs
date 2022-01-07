using System.Collections.Generic;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace ActiveStudy.Storage.Mongo.Materials.FlashCards;

public class FlashCardEntity
{
    public ObjectId Id { get; set; }

    [BsonElement("term")]
    public string Term { get; set; }

    [BsonElement("definition")]
    public string Definition { get; set; }

    [BsonElement("clues")]
    public IEnumerable<ClueEntity> Clues { get; set; }
}