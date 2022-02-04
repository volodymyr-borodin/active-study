using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace ActiveStudy.Storage.Mongo.Crm;

public class SchoolSubjectEntity
{
    public ObjectId Id { get; set; }

    [BsonElement("schoolId")]
    public string SchoolId { get; set; }

    [BsonElement("title")]
    public string Title { get; set; }
}