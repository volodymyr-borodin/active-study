using System.Collections.Generic;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace ActiveStudy.Storage.Mongo.Materials.FlashCards;

public class FlashCardsSetEntity
{
    public ObjectId Id { get; set; }

    [BsonElement("description")]
    public string Description { get; set; }

    [BsonElement("title")]
    public string Title { get; set; }

    [BsonElement("cards")]
    public IEnumerable<FlashCardEntity> Cards { get; set; }
}