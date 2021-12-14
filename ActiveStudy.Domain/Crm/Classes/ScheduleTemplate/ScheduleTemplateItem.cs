using System;

namespace ActiveStudy.Domain.Crm.Classes.ScheduleTemplate;

public record ScheduleTemplateItem(TimeOnly Start, TimeOnly End, Subject Subject);