using System;
using System.Collections.Generic;

namespace ActiveStudy.Domain.Crm.Scheduler;

public record EducationPeriod(string Id, string SchoolId, DateOnly From, DateOnly To, Dictionary<int, LessonDuration> Lessons);