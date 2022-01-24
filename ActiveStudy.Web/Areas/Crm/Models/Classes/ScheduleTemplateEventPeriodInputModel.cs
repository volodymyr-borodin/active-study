using System;
using System.Collections.Generic;

namespace ActiveStudy.Web.Areas.Crm.Models.Classes;

public record ScheduleTemplateEventPeriodInputModel(
    TimeOnly Start,
    TimeOnly End,
    Dictionary<DayOfWeek, ScheduleTemplateItemInputModel> Lessons);
