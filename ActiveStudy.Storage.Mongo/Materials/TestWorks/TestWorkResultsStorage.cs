using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ActiveStudy.Domain.Materials.TestWorks;
using ActiveStudy.Domain.Materials.TestWorks.Questions.MultiAnswer;
using ActiveStudy.Domain.Materials.TestWorks.Questions.SingleAnswer;
using ActiveStudy.Domain.Materials.TestWorks.Results;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Driver;

namespace ActiveStudy.Storage.Mongo.Materials.TestWorks;

public class TestWorkResultsStorage : ITestWorkResultsStorage
{
    private readonly MaterialsContext context;

    public TestWorkResultsStorage(MaterialsContext context)
    {
        this.context = context;
    }

    public async Task InsertAsync(TestWorkResult result)
    {
        var entity = new TestWorkResultEntity
        {
            TestWorkId = result.TestWorkId,
            VariantId = result.VariantId,
            Answers = result.Answers.Select(answer => new TestWorkResultAnswerEntity
            {
                QuestionId = answer.Question.Id,
                Info = answer switch
                {
                    TestWorkMultiQuestionResult testWorkMultiQuestionResult => new MultiAnswerTestWorkResultEntity
                    {
                        OptionIds = testWorkMultiQuestionResult.OptionIds
                    }.ToBsonDocument(),
                    TestWorkSingleQuestionResult testWorkSingleQuestionResult => new SingleAnswerTestWorkResultEntity
                    {
                        OptionId = testWorkSingleQuestionResult.OptionId
                    }.ToBsonDocument(),
                    _ => throw new ArgumentOutOfRangeException(nameof(answer))
                }
            }),
            Author = new TestWorkResultAuthorEntity
            {
                FirstName = result.Author.FirstName,
                LastName = result.Author.LastName,
                Email = result.Author.Email
            },
            CreatedOn = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds()
        };

        await context.TestWorkResults.InsertOneAsync(entity);
    }

    public async Task<IEnumerable<TestWorkResult>> FindAsync(string testWorkId)
    {
        var testWorkFilter = Builders<TestWorkEntity>.Filter.Eq(e => e.Id, new ObjectId(testWorkId));
        var testWork = (TestWork)await context.TestWorks.Find(testWorkFilter).FirstAsync();

        var filter = Builders<TestWorkResultEntity>.Filter.Eq(e => e.TestWorkId, testWorkId);
        var results = await context.TestWorkResults.Find(filter).ToListAsync();

        return results.Select(result =>
        {
            var questionsMap = testWork.Variants.First(v => v.Id == result.VariantId).Questions.ToDictionary(q => q.Id);

            var answers = result.Answers
                .Select(answer => questionsMap[answer.QuestionId] switch
                {
                    MultiAnswerQuestion multiAnswerQuestion => (TestWorkQuestionResult)new TestWorkMultiQuestionResult(multiAnswerQuestion, BsonSerializer.Deserialize<MultiAnswerTestWorkResultEntity>(answer.Info).OptionIds),
                    SingleAnswerQuestion singleAnswerQuestion => new TestWorkSingleQuestionResult(singleAnswerQuestion, BsonSerializer.Deserialize<SingleAnswerTestWorkResultEntity>(answer.Info).OptionId),
                    _ => throw new ArgumentOutOfRangeException()
                })
                .ToList();

            var author = new TestWorkResultAuthor(result.Author.FirstName, result.Author.LastName, result.Author.Email);
            
            return new TestWorkResult(result.TestWorkId, result.VariantId, answers, author, DateTimeOffset.FromUnixTimeMilliseconds(result.CreatedOn));
        }).ToList();
    }
}