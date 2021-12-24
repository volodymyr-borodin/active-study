using System;
using System.Collections.Generic;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace ActiveStudy.Storage.Mongo.Crm;

public class ScheduleTemplateEntity
{
    public ObjectId Id { get; set; }

    [BsonElement("classId")]
    public ObjectId ClassId { get; set; }
    
    [BsonElement("effectiveFrom")]
    public DateTime EffectiveFrom { get; set; }

    [BsonElement("effectiveTo")]
    public DateTime EffectiveTo { get; set; }

    [BsonElement("items")]
    public List<ScheduleTemplatePeriodEntity> Periods { get; set; }
}