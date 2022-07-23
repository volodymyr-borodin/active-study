using System;
using System.Collections.Generic;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace ActiveStudy.Storage.Mongo.Crm;

public class SchedulePeriodEntity
{
    public ObjectId Id { get; set; }

    [BsonElement("schoolId")]
    public ObjectId SchoolId { get; set; }

    [BsonElement("from")]
    public DateTime From { get; set; }

    [BsonElement("to")]
    public DateTime To { get; set; }

    [BsonElement("lessons")]
    public Dictionary<string, ScheduleLessonDurationEntity> Lessons { get; set; }
}