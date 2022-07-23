using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ActiveStudy.Domain.Crm.Scheduler;

public interface ISchedulerStorage
{
    Task<SchoolClassesSchedule> GetSchoolClassScheduleAsync(string schoolId);
    Task<ClassSchedule> GetClassScheduleAsync(string schoolId, ClassShortInfo @class);
    Task InsertScheduleAsync(SchoolClassesSchedule schedule);
}

public class DaySchedule : Dictionary<int, ScheduleItem>
{
    public DaySchedule(Dictionary<int, ScheduleItem> dictionary) : base(dictionary)
    {
    }

    public static DaySchedule Init(IEnumerable<int> durations, Dictionary<int, ScheduleItem> dictionary)
    {
        var dict = new Dictionary<int, ScheduleItem>();

        foreach (var (dayOfWeek, daySchedule) in dictionary)
        {
            dict[dayOfWeek] = daySchedule;
        }

        return new DaySchedule(dict);
    }

    public static DaySchedule Empty => new DaySchedule(new Dictionary<int, ScheduleItem>());
}

public class ClassSchedule : Dictionary<DayOfWeek, DaySchedule>
{
    public EducationPeriod EducationPeriod { get; }
    
    public ClassSchedule(EducationPeriod educationPeriod, Dictionary<DayOfWeek, DaySchedule> dictionary) : base(dictionary)
    {
        EducationPeriod = educationPeriod;
    }

    public static ClassSchedule Init(EducationPeriod educationPeriod, Dictionary<DayOfWeek, DaySchedule> dictionary)
    {
        var dict = new Dictionary<DayOfWeek, DaySchedule>
        {
            [DayOfWeek.Monday] = DaySchedule.Empty,
            [DayOfWeek.Tuesday] = DaySchedule.Empty,
            [DayOfWeek.Wednesday] = DaySchedule.Empty,
            [DayOfWeek.Thursday] = DaySchedule.Empty,
            [DayOfWeek.Friday] = DaySchedule.Empty,
            [DayOfWeek.Saturday] = DaySchedule.Empty,
            [DayOfWeek.Sunday] = DaySchedule.Empty
        };

        foreach (var (dayOfWeek, daySchedule) in dictionary)
        {
            dict[dayOfWeek] = daySchedule;
        }

        return new ClassSchedule(educationPeriod, dict);
    }
}

public record EducationPeriod(string Id, string SchoolId, DateOnly From, DateOnly To, Dictionary<int, LessonDuration> Lessons);

public class SchoolClassesSchedule : Dictionary<ClassShortInfo, ClassSchedule>
{
    public EducationPeriod EducationPeriod { get; }

    public SchoolClassesSchedule(EducationPeriod educationPeriod, Dictionary<ClassShortInfo, ClassSchedule> dictionary) : base(dictionary)
    {
        EducationPeriod = educationPeriod;
    }
}
