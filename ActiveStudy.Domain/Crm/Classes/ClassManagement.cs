using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ActiveStudy.Domain.Crm.Classes.ScheduleTemplate;
using ActiveStudy.Domain.Crm.Scheduler;
using Domain;
using SchedulePeriod = ActiveStudy.Domain.Crm.Scheduler.SchedulePeriod;

namespace ActiveStudy.Domain.Crm.Classes;

public class ClassManager
{
    private readonly IClassStorage classStorage;

    public ClassManager(IClassStorage classStorage)
    {
        this.classStorage = classStorage;
    }

    public async Task<Schedule> GetScheduleAsync(string classId, DateOnly from, DateOnly to)
    {
        var @class = await classStorage.GetByIdAsync(classId);
        var scheduleTemplate = await classStorage.GetScheduleTemplateAsync(classId);
        if (scheduleTemplate == null)
        {
            return Schedule.Empty(from, to);
        }

        var periods = scheduleTemplate.Periods.Select(p => new SchedulePeriod(p.Start, p.End));

        var dict = DaysRange(from, to)
            .ToDictionary(day => day, day =>
            {
                // TODO: Validate EffectiveFrom/EffectiveTo dates
                return (IReadOnlyCollection<Event>) scheduleTemplate.Periods
                    .Select(p => p.Lessons[day.DayOfWeek] == null ? null : @class.CreateEvent(day, p.Start, p.End, p.Lessons[day.DayOfWeek].Teacher, p.Lessons[day.DayOfWeek].Subject))
                    .ToList();
            });

        return new Schedule(dict, periods);
    }

    public async Task<DomainResult> SaveScheduleTemplateAsync(Class @class, ClassScheduleTemplate schedule)
    {
        await classStorage.SaveScheduleTemplateAsync(@class.Id, schedule);
        return DomainResult.Success();
    }

    private static IEnumerable<DateOnly> DaysRange(DateOnly from, DateOnly to)
    {
        while (from < to)
        {
            yield return from;
            from = from.AddDays(1);
        }
    }
}

public static class ClassExtensions
{
    public static Event CreateEvent(this Class @class, DateOnly day, TimeOnly from, TimeOnly to, TeacherShortInfo teacher, Subject subject)
    {
        return new Event(null, @class.SchoolId, string.Empty, teacher, subject, (ClassShortInfo) @class, day, from, to);
    }
}