using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ActiveStudy.Domain.Materials.TestWorks.Questions.SingleAnswer;
using Domain;

namespace ActiveStudy.Domain.Materials.TestWorks;

public class TestWorksService
{
    public async Task<TestWork> GetByIdAsync(string id)
    {
        var question = new SingleAnswerQuestion("1", "2+2=", "", 1, new[]
        {
            new SingleAnswerOption("1", "1"),
            new SingleAnswerOption("2", "2"),
            new SingleAnswerOption("3", "3"),
            new SingleAnswerOption("4", "4"),
        }, "2");

        var variant = new[] {new TestWorkVariant("v1", new[] {question})};

        return new TestWork("123", "Titile 123", "", new Subject("1", "subject 1"), new User("asd", "asdasdasd"),
            variant, TestWorkStatus.Published);

        // return new TestWork("1", "title1", "description1", new Subject("1", "subject 1"), new User("asd", "asdasdasd"),
        //     Enumerable.Empty<TestWorkVariant>(), TestWorkStatus.Published);
    }

    public async Task<IEnumerable<TestWork>> FindPublishedAsync()
    {
        return new[]
        {
            new TestWork("1", "title1", "description1", new Subject("1", "subject 1"), new User("asd", "asdasdasd"), Enumerable.Empty<TestWorkVariant>(), TestWorkStatus.Published),
            new TestWork("2", "title2", "description2", new Subject("2", "subject 2"), new User("asd", "asdasdasd"), Enumerable.Empty<TestWorkVariant>(), TestWorkStatus.Published)
        };
    }

    public async Task<DomainResult> CreateAsync(TestWorkDetails testWork)
    {
        // var id = await _testWorkStorage.InsertAsync(test);

        return DomainResult.Success();
    }
}
