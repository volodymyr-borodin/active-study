using System;
using System.Collections.Generic;

namespace ActiveStudy.Web.Models.Classes;

public record ClassScheduleTemplateInputModel(
    DateOnly EffectiveFrom,
    DateOnly EffectiveTo,
    IEnumerable<ScheduleTemplateEventPeriodInputModel> Periods,
    IEnumerable<ScheduleTemplateItemInputModel> Items);