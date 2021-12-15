using System;

namespace ActiveStudy.Web.Models.Classes;

public record ScheduleTemplateItemInputModel(DayOfWeek DayOfWeek, int Order, int SubjectId, int TeacherId);