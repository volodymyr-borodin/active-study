using System;
using System.Collections.Generic;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace ActiveStudy.Storage.Mongo.Crm;

internal class SchedulePeriodEntity
{
    public ObjectId Id { get; set; }

    [BsonElement("from")]
    public DateTime From { get; set; }

    [BsonElement("to")]
    public DateTime To { get; set; }

    [BsonElement("lessons")]
    public Dictionary<int, ScheduleLessonDurationEntity> Lessons { get; set; }
}