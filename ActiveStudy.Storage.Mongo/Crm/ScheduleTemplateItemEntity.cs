using System;
using System.Collections.Generic;
using MongoDB.Bson.Serialization.Attributes;

namespace ActiveStudy.Storage.Mongo.Crm;

public class ScheduleTemplateLessonEntity
{
    [BsonElement("teacher")]
    public TeacherShortEntity Teacher { get; set; }

    [BsonElement("subject")]
    public SubjectEntity Subject { get; set; }
}

public class ScheduleTemplatePeriodEntity
{
    [BsonElement("start")]
    public TimeSpan Start { get; set; }

    [BsonElement("end")]
    public TimeSpan End { get; set; }

    [BsonElement("lessons")]
    public Dictionary<string, ScheduleTemplateLessonEntity> Lessons { get; set; }
}
