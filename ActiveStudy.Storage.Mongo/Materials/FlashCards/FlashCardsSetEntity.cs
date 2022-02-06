using System.Collections.Generic;
using ActiveStudy.Domain.Materials.FlashCards;
using ActiveStudy.Storage.Mongo.Crm;
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

    [BsonElement("author")]
    public UserEntity Author { get; set; }

    [BsonElement("cards")]
    public IEnumerable<FlashCardEntity> Cards { get; set; }

    [BsonElement("status")]
    public FlashCardSetStatus Status { get; set; }
}