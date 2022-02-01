using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace ActiveStudy.Storage.Mongo.Materials.FlashCards;

public class UserCardProgressEntity
{
    public ObjectId Id { get; set; }

    [BsonElement("userId")]
    public string UserId { get; set; }
    
    [BsonElement("termId")]
    public string TermId { get; set; }

    [BsonElement("progress")]
    public int Progress { get; set; }
    
    [BsonElement("updatedOn")]
    public long UpdatedOn { get; set; }
}