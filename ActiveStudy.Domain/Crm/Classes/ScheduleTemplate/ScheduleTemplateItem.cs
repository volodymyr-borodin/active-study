using System;

namespace ActiveStudy.Domain.Crm.Classes.ScheduleTemplate;

public record ScheduleTemplateItem(TimeOnly Start, TimeOnly End, ClassShortInfo Class, TeacherShortInfo Teacher, Subject Subject);
