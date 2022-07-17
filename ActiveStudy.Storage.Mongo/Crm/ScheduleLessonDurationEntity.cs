using System;

namespace ActiveStudy.Storage.Mongo.Crm;

public class ScheduleLessonDurationEntity
{
    public TimeSpan Start { get; set; }

    public TimeSpan End { get; set; }
}