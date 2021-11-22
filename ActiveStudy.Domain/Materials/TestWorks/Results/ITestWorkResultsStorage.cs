using System.Threading.Tasks;

namespace ActiveStudy.Domain.Materials.TestWorks.Results;

public interface ITestWorkResultsStorage
{
    Task InsertAsync(TestWorkResult result);
}
