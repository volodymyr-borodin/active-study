using System;
using System.Collections.Generic;
using ActiveStudy.Domain.Crm.Classes;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace ActiveStudy.Web.Areas.Crm.Models.Classes;

public record ClassScheduleTemplateViewModel(
    Class Class,
    IEnumerable<SelectListItem> Teachers,
    IEnumerable<SelectListItem> Subjects,
    DateOnly EffectiveFrom,
    DateOnly EffectiveTo,
    List<ScheduleTemplateEventPeriodInputModel> Periods) : ClassScheduleTemplateInputModel(
    EffectiveFrom,
    EffectiveTo,
    Periods)
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