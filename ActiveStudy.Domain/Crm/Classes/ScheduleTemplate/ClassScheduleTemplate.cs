using System;
using System.Collections.Generic;
using System.Linq;
using Domain;

namespace ActiveStudy.Domain.Crm.Classes.ScheduleTemplate;

public class ClassScheduleTemplate
{
    private ClassScheduleTemplate(
        DateOnly effectiveFrom,
        DateOnly effectiveTo,
        IEnumerable<ScheduleTemplateDay> days)
    {
        EffectiveFrom = effectiveFrom;
        EffectiveTo = effectiveTo;
        Days = days;
    }

    public static (ClassScheduleTemplate, DomainResult) New(
        DateOnly effectiveFrom,
        DateOnly effectiveTo,
        ICollection<ScheduleTemplateDay> days)
    {
        if (effectiveFrom < effectiveTo)
        {
            return (null, DomainResult.Error($"{nameof(effectiveFrom)} can't be greater than {nameof(effectiveTo)}"));
        }

        if (days.Count == 0)
        {
            return (null, DomainResult.Error("Days can't be empty"));
        }

        if (days.Count != days.DistinctBy(d => d.DayOfWeek).Count())
        {
            return (null, DomainResult.Error("DayOfWeeks should be uniq"));
        }

        return new(
            new ClassScheduleTemplate(effectiveFrom, effectiveTo, days),
            DomainResult.Success());
    }

    public DateOnly EffectiveFrom { get; }
    public DateOnly EffectiveTo { get; }
    public IEnumerable<ScheduleTemplateDay> Days { get; }
}
