using System;
using System.Collections.Generic;

namespace ActiveStudy.Web.Models.Classes;

public record ClassScheduleTemplateInputModel(DateOnly EffectiveFrom, DateOnly EffectiveTo, IEnumerable<ScheduleTemplateItemInputModel> Items);

public record ScheduleTemplateItemInputModel(DayOfWeek DayOfWeek, TimeOnly Start, TimeOnly End);
