using System;
using System.Collections.Generic;
using Domain;

namespace ActiveStudy.Domain.Crm.Classes.ScheduleTemplate;

public class ClassScheduleTemplate
{
    private ClassScheduleTemplate(
        DateOnly effectiveFrom,
        DateOnly effectiveTo,
        List<SchedulePeriod> periods)
    {
        EffectiveFrom = effectiveFrom;
        EffectiveTo = effectiveTo;
        Periods = periods;
    }

    public static (ClassScheduleTemplate, DomainResult) New(
        DateOnly effectiveFrom,
        DateOnly effectiveTo,
        List<SchedulePeriod> periods)
    {
        if (effectiveFrom >= effectiveTo)
        {
            return (null, DomainResult.Error($"{nameof(effectiveFrom)} can't be greater than {nameof(effectiveTo)}"));
        }

        return new(
            new ClassScheduleTemplate(effectiveFrom, effectiveTo, periods),
            DomainResult.Success());
    }

    public DateOnly EffectiveFrom { get; }
    public DateOnly EffectiveTo { get; }
    public List<SchedulePeriod> Periods { get; }
}
