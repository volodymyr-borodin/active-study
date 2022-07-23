using System.Threading.Tasks;

namespace ActiveStudy.Domain.Crm.Scheduler;

public interface ISchedulerStorage
{
    Task<SchoolClassesSchedule> GetSchoolClassScheduleAsync(string schoolId);
    Task<ClassSchedule> GetClassScheduleAsync(string schoolId, ClassShortInfo @class);
    Task<TeacherSchedule> GetTeacherScheduleAsync(string schoolId, TeacherShortInfo teacher);
    Task InsertScheduleAsync(SchoolClassesSchedule schedule);
}