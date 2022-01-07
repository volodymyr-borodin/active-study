using MongoDB.Bson.Serialization.Attributes;

namespace ActiveStudy.Storage.Mongo.Materials.FlashCards;

public class ClueEntity
{
    [BsonElement("text")]
    public string Text { get; set; }
}