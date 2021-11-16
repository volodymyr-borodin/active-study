using System.Collections.Generic;
using System.Threading.Tasks;

namespace ActiveStudy.Domain.Materials.TestWorks;

public interface ITestWorksStorage
{
    Task<TestWork> GetByIdAsync(string id);
    Task<IEnumerable<TestWork>> FindAsync();
    Task InsertAsync(TestWorkDetails testWork);
}
