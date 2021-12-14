using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ActiveStudy.Domain.Crm.Classes.ScheduleTemplate;
using ActiveStudy.Domain.Crm.Scheduler;

namespace ActiveStudy.Domain.Crm.Classes;

public class ClassManager
{
    private readonly IEventsStorage eventsStorage;
    private readonly ISubjectStorage subjectStorage;

    public ClassManager(
        IEventsStorage eventsStorage,
        ISubjectStorage subjectStorage)
    {
        this.eventsStorage = eventsStorage;
        this.subjectStorage = subjectStorage;
    }
    
    public async Task<Schedule> GetScheduleAsync(string classId, DateOnly from, DateOnly to)
    {
        var events = (await eventsStorage.GetByClassAsync(classId, from, to))
            .GroupBy(@event => @event.Date)
            .ToDictionary(grouping => grouping.Key, grouping => grouping.ToList());
        
        var scheduleTemplate = ClassScheduleTemplate.New(
            new DateOnly(2021, 9, 1),
            new DateOnly(2022, 5, 30),
            new []
            {
                new ScheduleTemplateDay(DayOfWeek.Monday, new []
                {
                    new ScheduleTemplateItem(new TimeOnly(8, 30), new TimeOnly(9, 15), await subjectStorage.GetByIdAsync("ua-fizika")),
                    new ScheduleTemplateItem(new TimeOnly(8, 30), new TimeOnly(9, 15), await subjectStorage.GetByIdAsync("ua-himiya")),
                    new ScheduleTemplateItem(new TimeOnly(8, 30), new TimeOnly(9, 15), await subjectStorage.GetByIdAsync("ua-biologiya")),
                    new ScheduleTemplateItem(new TimeOnly(8, 30), new TimeOnly(9, 15), await subjectStorage.GetByIdAsync("ua-informatika"))
                }),
                new ScheduleTemplateDay(DayOfWeek.Tuesday, new []
                {
                    new ScheduleTemplateItem(new TimeOnly(8, 30), new TimeOnly(9, 15), await subjectStorage.GetByIdAsync("ua-fizika")),
                    new ScheduleTemplateItem(new TimeOnly(8, 30), new TimeOnly(9, 15), await subjectStorage.GetByIdAsync("ua-himiya")),
                    new ScheduleTemplateItem(new TimeOnly(8, 30), new TimeOnly(9, 15), await subjectStorage.GetByIdAsync("ua-biologiya")),
                    new ScheduleTemplateItem(new TimeOnly(8, 30), new TimeOnly(9, 15), await subjectStorage.GetByIdAsync("ua-informatika"))
                })
            });

        var dict = DaysRange(from, to)
            .ToDictionary(day => day, day =>
            {
                
            });

        return new Schedule(dict);
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