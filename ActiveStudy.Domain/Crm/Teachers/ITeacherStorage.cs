using System.Collections.Generic;
using System.Threading.Tasks;

namespace ActiveStudy.Domain.Crm.Teachers
{
    public interface ITeacherStorage
    {
        Task<Teacher> GetByIdAsync(string id);
        Task<IEnumerable<Teacher>> FindAsync(string schoolId);
        Task<string> InsertAsync(Teacher teacher);
        Task DeleteAsync(Teacher teacher);
        Task SetUserIdAsync(string id, string userId);
    }
}
