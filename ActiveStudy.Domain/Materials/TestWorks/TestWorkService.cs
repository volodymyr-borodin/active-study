using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ActiveStudy.Domain.Materials.TestWorks.Questions.SingleAnswer;
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

    public Task<IEnumerable<TestWork>> FindPublishedAsync()
    {
        return storage.FindAsync();
    }

    public async Task<DomainResult> CreateAsync(TestWorkDetails testWork)
    {
        await storage.InsertAsync(testWork);

        return DomainResult.Success();
    }
}
