using System;

namespace ActiveStudy.Web.Models.Classes;

public record ScheduleTemplateEventPeriodInputModel(TimeOnly Start, TimeOnly End);