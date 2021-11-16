using System.Collections.Generic;
using System.Linq;
using ActiveStudy.Domain.Materials.TestWorks;
using MongoDB.Bson.Serialization.Attributes;

namespace ActiveStudy.Storage.Mongo.Materials;

public class TestWorkVariantEntity
{
    [BsonElement("id")]
    public string Id { get; set; }

    [BsonElement("questions")]
    public IEnumerable<QuestionDetailsEntity> Questions { get; set; }

    public static implicit operator TestWorkVariant(TestWorkVariantEntity entity)
    {
        return new TestWorkVariant(entity.Id,
            entity.Questions.Select(q => (Question)q).ToList());
    }
}