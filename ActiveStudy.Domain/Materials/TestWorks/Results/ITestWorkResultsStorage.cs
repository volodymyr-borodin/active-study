using System.Collections.Generic;
using System.Threading.Tasks;

namespace ActiveStudy.Domain.Materials.TestWorks.Results;

public interface ITestWorkResultsStorage
{
    Task InsertAsync(TestWorkResult result);
    Task<IEnumerable<TestWorkResult>> FindAsync(string testWorkId);
}
