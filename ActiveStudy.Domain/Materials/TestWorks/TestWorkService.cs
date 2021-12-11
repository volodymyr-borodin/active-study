using System.Collections.Generic;
using System.Threading.Tasks;
using Domain;

namespace ActiveStudy.Domain.Materials.TestWorks;

public class TestWorksService
{
    private readonly ITestWorksStorage storage;

    public TestWorksService(ITestWorksStorage storage)
    {
        this.storage = storage;
    }

    public Task<TestWork> GetByIdAsync(string id)
    {
        return storage.GetByIdAsync(id);
    }

    public Task<IEnumerable<TestWork>> FindPublishedAsync(string categoryId)
    {
        return storage.FindAsync(categoryId);
    }

    public Task<IEnumerable<TestWork>> FindByAuthorAsync(string authorId)
    {
        return storage.FindByAuthorAsync(authorId);
    }

    public async Task<DomainResult> CreateAsync(TestWorkDetails testWork)
    {
        await storage.InsertAsync(testWork);

        return DomainResult.Success();
    }
}
