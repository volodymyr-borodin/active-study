using System.Collections.Generic;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace ActiveStudy.Storage.Mongo.Materials;

public class TestWorkResultEntity
{
    public ObjectId Id { get; set; }

    [BsonElement("testWorkId")]
    public string TestWorkId { get; set; }

    [BsonElement("variantId")]
    public string VariantId { get; set; }

    [BsonElement("answers")]
    public IEnumerable<TestWorkResultAnswerEntity> Answers { get; set; }

    [BsonElement("author")]
    public TestWorkResultAuthorEntity Author { get; set; }

    [BsonElement("createdOn")]
    public long CreatedOn { get; set; }
}

public class SingleAnswerTestWorkResultEntity
{
    [BsonElement("optionId")]
    public string OptionId { get; set; }
}

public class MultiAnswerTestWorkResultEntity
{
    [BsonElement("optionIds")]
    public IEnumerable<string> OptionIds { get; set; }
}

public class TestWorkResultAnswerEntity
{
    [BsonElement("questionId")]
    public string QuestionId { get; set; }

    [BsonElement("info")]
    public BsonDocument Info { get; set; }
}

public class TestWorkResultAuthorEntity
{
    public string FirstName { get; set; }

    public string LastName { get; set; }

    public string Email { get; set; }
}
