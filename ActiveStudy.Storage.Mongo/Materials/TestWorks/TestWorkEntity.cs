using System.Collections.Generic;
using System.Linq;
using ActiveStudy.Domain;
using ActiveStudy.Domain.Materials.TestWorks;
using ActiveStudy.Storage.Mongo.Crm;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace ActiveStudy.Storage.Mongo.Materials.TestWorks;

public class TestWorkEntity
{
    public ObjectId Id { get; set; }

    [BsonElement("title")]
    public string Title { get; set; }

    [BsonElement("description")]
    public string Description { get; set; }

    [BsonElement("subject")]
    public SubjectEntity Subject { get; set; }

    [BsonElement("author")]
    public UserEntity Author { get; set; }

    [BsonElement("variants")]
    public IEnumerable<TestWorkVariantEntity> Variants { get; set; }

    [BsonElement("status")]
    public TestWorkStatus Status { get; set; }

    public static explicit operator TestWork(TestWorkEntity entity)
    {
        return new TestWork(entity.Id.ToString(),
            entity.Title,
            entity.Description,
            (Subject) entity.Subject,
            entity.Author,
            entity.Variants.Select(v => (TestWorkVariant)v),
            entity.Status);
    }
}