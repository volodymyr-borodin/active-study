using System.Collections.Generic;
using System.Threading.Tasks;

namespace ActiveStudy.Domain.Crm.Relatives
{
    public interface IRelativesStorage
    {
        Task<string> InsertAsync(Relative relative);
        Task AddStudentAsync(string relativeId, string studentId);
        Task<IEnumerable<Relative>> SearchAsync(string studentId);
    }
}