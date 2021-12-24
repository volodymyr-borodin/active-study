using System;
using System.Collections.Generic;

namespace ActiveStudy.Domain.Crm.Classes.ScheduleTemplate;

public record ScheduleTemplateLesson(ClassShortInfo Class, TeacherShortInfo Teacher, Subject Subject);

public record SchedulePeriod(TimeOnly Start, TimeOnly End, Dictionary<DayOfWeek, ScheduleTemplateLesson> Lessons);
