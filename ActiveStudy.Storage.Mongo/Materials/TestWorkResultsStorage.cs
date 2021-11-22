using System;
using System.Linq;
using System.Threading.Tasks;
using ActiveStudy.Domain.Materials.TestWorks.Results;
using MongoDB.Bson;

namespace ActiveStudy.Storage.Mongo.Materials;

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
                QuestionType = answer.Question.GetType().Name,
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
            }
        };

        await context.TestWorkResults.InsertOneAsync(entity);
    }
}