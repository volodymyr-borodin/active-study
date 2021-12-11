using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ActiveStudy.Domain.Materials.TestWorks;
using ActiveStudy.Domain.Materials.TestWorks.Questions.MultiAnswer;
using ActiveStudy.Domain.Materials.TestWorks.Questions.SingleAnswer;
using MongoDB.Bson;
using MongoDB.Driver;

namespace ActiveStudy.Storage.Mongo.Materials;

public class TestWorksStorage : ITestWorksStorage
{
    private readonly MaterialsContext context;

    public TestWorksStorage(MaterialsContext context)
    {
        this.context = context;
    }

    public async Task<TestWork> GetByIdAsync(string id)
    {
        var filter = Builders<TestWorkEntity>.Filter.Eq(t => t.Id, ObjectId.Parse(id));

        var entity = await context.TestWorks
            .Find(filter)
            .FirstOrDefaultAsync();

        return (TestWork)entity;
    }

    public async Task<IEnumerable<TestWork>> FindAsync(string categoryId)
    {
        var filter = Builders<TestWorkEntity>.Filter.Eq(tw => tw.Status, TestWorkStatus.Published);

        if (!string.IsNullOrEmpty(categoryId))
        {
            filter &= Builders<TestWorkEntity>.Filter.Eq(tw => tw.Subject.Id, categoryId);
        }
        
        var entities = await context.TestWorks
            .Find(filter)
            .ToListAsync();

        return entities.Select(testWork => (TestWork)testWork).ToList();
    }

    public async Task<IEnumerable<TestWork>> FindByAuthorAsync(string authorId)
    {
        var filter = Builders<TestWorkEntity>.Filter.Eq(tw => tw.Author.UserId, authorId);

        var entities = await context.TestWorks
            .Find(filter)
            .ToListAsync();

        return entities.Select(testWork => (TestWork)testWork).ToList();
    }

    public async Task InsertAsync(TestWorkDetails testWork)
    {
        var entity = new TestWorkEntity
        {
            Title = testWork.Title,
            Description = testWork.Description,
            Subject = testWork.Subject,
            Author = testWork.Author,
            Variants = testWork.Variants.Select(v => new TestWorkVariantEntity
            {
                Id = v.Id,
                Questions = v.Questions.Select(q => new QuestionDetailsEntity
                {
                    Id = q.Id,
                    Text = q.Text,
                    Description = q.Description,
                    MaxScore = q.MaxScore,
                    Type = q.GetType().Name,
                    Info = q switch
                    {
                        SingleAnswerQuestion singleAnswerQuestion => new SingleAnswerInfoEntity
                        {
                            CorrectAnswerId = singleAnswerQuestion.CorrectAnswerId,
                            Options = singleAnswerQuestion.Options.Select(o => new SingleAnswerOptionEntity
                            {
                                Id = o.Id,
                                Text = o.Text
                            }).ToList()
                        }.ToBsonDocument(),
                        MultiAnswerQuestion multiAnswerQuestion => new MultiAnswerInfoEntity
                        {
                            Options = multiAnswerQuestion.Options.Select(o => new MultiAnswerOptionEntity
                                {
                                    Id = o.Id,
                                    Text = o.Text,
                                    IsCorrect = o.IsCorrect
                                }).ToList()
                        }.ToBsonDocument(),
                        _ => throw new ArgumentOutOfRangeException(nameof(q))
                    }
                })
            }),
            Status = testWork.Status
        };

        await context.TestWorks.InsertOneAsync(entity);
    }
}