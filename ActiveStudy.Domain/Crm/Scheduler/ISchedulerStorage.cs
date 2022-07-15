using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ActiveStudy.Domain.Crm.Scheduler;

using ClassSchedule = Dictionary<DayOfWeek, Dictionary<LessonDuration, ScheduleItem>>;
using SchoolSchedule = Dictionary<ClassShortInfo, Dictionary<DayOfWeek, Dictionary<LessonDuration, ScheduleItem>>>;

public interface ISchedulerStorage
{
    Task<SchoolSchedule> GetClassBasedSchoolScheduleAsync();
    Task<ClassSchedule> GetClassScheduleAsync(ClassShortInfo classShortInfo);
}
