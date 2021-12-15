using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ActiveStudy.Domain.Crm.Classes.ScheduleTemplate;
using ActiveStudy.Domain.Crm.Scheduler;
using Domain;

namespace ActiveStudy.Domain.Crm.Classes;

public class ClassManager
{
    private readonly ISubjectStorage subjectStorage;
    private readonly IClassStorage classStorage;

    public ClassManager(
        ISubjectStorage subjectStorage,
        IClassStorage classStorage)
    {
        this.subjectStorage = subjectStorage;
        this.classStorage = classStorage;
    }

    public async Task<Schedule> GetScheduleAsync(string classId, DateOnly from, DateOnly to)
    {
        var @class = await classStorage.GetByIdAsync(classId);
        var classInfo = (ClassShortInfo) @class;

        var teacher = new TeacherShortInfo("1", "Sadad Fdghkfdg", null);

        var (scheduleTemplate, _) = ClassScheduleTemplate.New(
            new DateOnly(2021, 9, 1),
            new DateOnly(2022, 5, 30),
            new Dictionary<DayOfWeek, IReadOnlyCollection<ScheduleTemplateItem>>
            {
                [DayOfWeek.Monday] = new []
                {
                    new ScheduleTemplateItem(new TimeOnly(8, 30), new TimeOnly(9, 15), classInfo, teacher, await subjectStorage.GetByIdAsync("ua-fizika")),
                    new ScheduleTemplateItem(new TimeOnly(9, 35), new TimeOnly(10, 20), classInfo, teacher, await subjectStorage.GetByIdAsync("ua-himiya")),
                    new ScheduleTemplateItem(new TimeOnly(10, 40), new TimeOnly(11, 25), classInfo, teacher, await subjectStorage.GetByIdAsync("ua-biologiya")),
                    new ScheduleTemplateItem(new TimeOnly(11, 45), new TimeOnly(12, 30), classInfo, teacher, await subjectStorage.GetByIdAsync("ua-informatika"))
                },
                [DayOfWeek.Tuesday] = new []
                {
                    new ScheduleTemplateItem(new TimeOnly(8, 30), new TimeOnly(9, 15), classInfo, teacher, await subjectStorage.GetByIdAsync("ua-fizika")),
                    new ScheduleTemplateItem(new TimeOnly(9, 35), new TimeOnly(10, 20), classInfo, teacher, await subjectStorage.GetByIdAsync("ua-himiya")),
                    new ScheduleTemplateItem(new TimeOnly(10, 40), new TimeOnly(11, 25), classInfo, teacher, await subjectStorage.GetByIdAsync("ua-biologiya")),
                    new ScheduleTemplateItem(new TimeOnly(11, 45), new TimeOnly(12, 30), classInfo, teacher, await subjectStorage.GetByIdAsync("ua-informatika"))
                }
            });

        var dict = DaysRange(from, to)
            .ToDictionary(day => day, day =>
            {
                // TODO: Validate EffectiveFrom/EffectiveTo dates
                var templateEvents = scheduleTemplate.Days.GetValueOrDefault(day.DayOfWeek) ?? Enumerable.Empty<ScheduleTemplateItem>();
                return (IReadOnlyCollection<Event>) templateEvents
                    .Select(i => @class.CreateEvent(day, i.Start, i.End, i.Teacher, i.Subject))
                    .ToList();
            });

        return new Schedule(dict);
    }

    public async Task<DomainResult> SaveScheduleTemplateAsync(Class @class, ClassScheduleTemplate schedule)
    {
        await classStorage.InsertScheduleTemplateAsync(@class.Id, schedule);
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