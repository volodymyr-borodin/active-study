using System;
using MongoDB.Bson.Serialization.Attributes;

namespace ActiveStudy.Storage.Mongo.Crm;

public class ScheduleTemplateItemEntity
{
    [BsonElement("dayOfWeek")]
    public DayOfWeek DayOfWeek { get; set; }

    [BsonElement("start")]
    public TimeOnly Start { get; set; }

    [BsonElement("end")]
    public TimeOnly End { get; set; }

    [BsonElement("class")]
    public ClassShortEntity Class { get; set; }

    [BsonElement("teacher")]
    public TeacherShortEntity Teacher { get; set; }

    [BsonElement("subject")]
    public SubjectEntity Subject { get; set; }
}