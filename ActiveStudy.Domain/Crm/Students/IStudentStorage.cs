using System.Collections.Generic;
using System.Threading.Tasks;

namespace ActiveStudy.Domain.Crm.Students
{
    public interface IStudentStorage
    {
        Task<string> InsertAsync(Student student);
        Task<IEnumerable<Student>> FindAsync(StudentFilter filter);
    }
}