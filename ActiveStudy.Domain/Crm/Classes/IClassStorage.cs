using System.Collections.Generic;
using System.Threading.Tasks;
using ActiveStudy.Domain.Crm.Classes.ScheduleTemplate;

namespace ActiveStudy.Domain.Crm.Classes
{
    public interface IClassStorage
    {
        Task<Class> GetByIdAsync(string id);
        Task<IEnumerable<Class>> FindAsync(string schoolId);
        Task<string> InsertAsync(Class @class);
        Task InsertManyAsync(IEnumerable<Class> classes);
        Task SetTeacherUserIdAsync(string teacherId, string userId);

        Task SaveScheduleTemplateAsync(string classId, ClassScheduleTemplate schedule);
        Task<ClassScheduleTemplate> GetScheduleTemplateAsync(string classId);
    }
}