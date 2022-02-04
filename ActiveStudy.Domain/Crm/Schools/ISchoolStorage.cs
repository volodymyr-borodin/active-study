using System.Collections.Generic;
using System.Threading.Tasks;

namespace ActiveStudy.Domain.Crm.Schools;

public interface ISchoolStorage
{
    Task<School> GetByIdAsync(string id);
    Task<IEnumerable<School>> SearchByIdsAsync(IEnumerable<string> ids);
    Task<string> CreateAsync(School school);

    Task<IEnumerable<Subject>> GetSubjectsAsync(string id, IEnumerable<string> subjectIds = null);
    Task InsertSubjectsAsync(string id, IEnumerable<Subject> subjects);
}