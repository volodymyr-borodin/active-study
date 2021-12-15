using System;
using System.Collections.Generic;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace ActiveStudy.Storage.Mongo.Crm;

public class ScheduleTemplateEntity
{
    public ObjectId Id { get; set; }

    [BsonElement("EffectiveFrom")]
    public DateOnly EffectiveFrom { get; set; }

    [BsonElement("effectiveTo")]
    public DateOnly EffectiveTo { get; set; }

    [BsonElement("items")]
    public List<ScheduleTemplateItemEntity> Items { get; set; }
}