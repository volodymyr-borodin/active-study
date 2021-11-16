using System.Collections.Generic;
using System.Linq;
using ActiveStudy.Domain.Materials.TestWorks;
using ActiveStudy.Domain.Materials.TestWorks.Questions.MultiAnswer;
using ActiveStudy.Domain.Materials.TestWorks.Questions.SingleAnswer;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Attributes;

namespace ActiveStudy.Storage.Mongo.Materials;

public class QuestionDetailsEntity
{
    [BsonElement("id")]
    public string Id { get; set; }

    [BsonElement("text")]
    public string Text { get; set; }

    [BsonElement("description")]
    public string Description { get; set; }

    [BsonElement("maxScore")]
    public decimal MaxScore { get; set; }
    
    [BsonElement("type")]
    public string Type { get; set; }

    [BsonElement("info")]
    public BsonDocument Info { get; set; }

    public static implicit operator Question(QuestionDetailsEntity entity)
    {
        switch (entity.Type) 
        {
            case nameof(SingleAnswerQuestion):
            {
                var info = BsonSerializer.Deserialize<SingleAnswerInfoEntity>(entity.Info);

                return new SingleAnswerQuestion(entity.Id,
                    entity.Text,
                    entity.Description,
                    entity.MaxScore,
                    info.Options.Select(o => (SingleAnswerOption)o).ToList(),
                    info.CorrectAnswerId);
            }
            case nameof(MultiAnswerQuestion):
            {
                var info = BsonSerializer.Deserialize<MultiAnswerInfoEntity>(entity.Info);

                return new MultiAnswerQuestion(entity.Id,
                    entity.Text,
                    entity.Description,
                    entity.MaxScore,
                    info.Options.Select(o => (MultiAnswerOption)o).ToList(),
                    info.CorrectAnswerIds);
            }
        }

        return null;
    }
}

public class MultiAnswerInfoEntity
{
    [BsonElement("correctAnswerIds")]
    public IEnumerable<string> CorrectAnswerIds { get; set; }

    [BsonElement("options")]
    public IEnumerable<MultiAnswerOptionEntity> Options { get; set; }
}

public class MultiAnswerOptionEntity
{
    [BsonElement("id")]
    public string Id { get; set; }

    [BsonElement("text")]
    public string Text { get; set; }

    public static implicit operator MultiAnswerOption(MultiAnswerOptionEntity entity)
    {
        return new MultiAnswerOption(entity.Id, entity.Text);
    }
}


public class SingleAnswerInfoEntity
{
    [BsonElement("correctAnswerId")]
    public string CorrectAnswerId { get; set; }

    [BsonElement("options")]
    public IEnumerable<SingleAnswerOptionEntity> Options { get; set; }
}

public class SingleAnswerOptionEntity
{
    [BsonElement("id")]
    public string Id { get; set; }

    [BsonElement("text")]
    public string Text { get; set; }

    public static implicit operator SingleAnswerOption(SingleAnswerOptionEntity entity)
    {
        return new SingleAnswerOption(entity.Id, entity.Text);
    }
}
