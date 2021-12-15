using System;
using System.Collections.Generic;
using Domain;

namespace ActiveStudy.Domain.Crm.Classes.ScheduleTemplate;

public class ClassScheduleTemplate
{
    private ClassScheduleTemplate(
        DateOnly effectiveFrom,
        DateOnly effectiveTo,
        IReadOnlyDictionary<DayOfWeek, IReadOnlyCollection<ScheduleTemplateItem>> days)
    {
        EffectiveFrom = effectiveFrom;
        EffectiveTo = effectiveTo;
        Days = days;
    }

    public static (ClassScheduleTemplate, DomainResult) New(
        DateOnly effectiveFrom,
        DateOnly effectiveTo,
        IReadOnlyDictionary<DayOfWeek, IReadOnlyCollection<ScheduleTemplateItem>> days)
    {
        if (effectiveFrom >= effectiveTo)
        {
            return (null, DomainResult.Error($"{nameof(effectiveFrom)} can't be greater than {nameof(effectiveTo)}"));
        }

        if (days.Count == 0)
        {
            return (null, DomainResult.Error("Days can't be empty"));
        }

        return new(
            new ClassScheduleTemplate(effectiveFrom, effectiveTo, days),
            DomainResult.Success());
    }

    public DateOnly EffectiveFrom { get; }
    public DateOnly EffectiveTo { get; }
    public IReadOnlyDictionary<DayOfWeek, IReadOnlyCollection<ScheduleTemplateItem>> Days { get; }
}
