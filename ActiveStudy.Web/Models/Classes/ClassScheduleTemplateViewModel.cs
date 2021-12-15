using System;
using System.Collections.Generic;
using ActiveStudy.Domain;
using ActiveStudy.Domain.Crm;

namespace ActiveStudy.Web.Models.Classes;

public record ClassScheduleTemplateViewModel(
    IEnumerable<TeacherShortInfo> Teachers,
    IEnumerable<Subject> Subjects,
    DateOnly EffectiveFrom,
    DateOnly EffectiveTo,
    IEnumerable<ScheduleTemplateEventPeriodInputModel> Periods,
    IEnumerable<ScheduleTemplateItemInputModel> Items) : ClassScheduleTemplateInputModel(EffectiveFrom, EffectiveTo,
    Periods, Items)
{
    public IEnumerable<DayOfWeek> DayOfWeeks => new[]
    {
        DayOfWeek.Monday,
        DayOfWeek.Tuesday,
        DayOfWeek.Wednesday,
        DayOfWeek.Thursday,
        DayOfWeek.Friday,
        DayOfWeek.Saturday,
        DayOfWeek.Sunday
    };
}