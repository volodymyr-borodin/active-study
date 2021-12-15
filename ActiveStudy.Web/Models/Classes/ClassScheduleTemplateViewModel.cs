using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace ActiveStudy.Web.Models.Classes;

public record ClassScheduleTemplateViewModel(
    IEnumerable<SelectListItem> Teachers,
    IEnumerable<SelectListItem> Subjects,
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