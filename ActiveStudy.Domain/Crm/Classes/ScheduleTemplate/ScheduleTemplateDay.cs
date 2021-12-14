using System;
using System.Collections.Generic;

namespace ActiveStudy.Domain.Crm.Classes.ScheduleTemplate;

public record ScheduleTemplateDay(DayOfWeek DayOfWeek, IEnumerable<ScheduleTemplateItem> Items);