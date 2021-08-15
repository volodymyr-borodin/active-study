using System;
using ActiveStudy.Domain.Crm.Scheduler;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace ActiveStudy.Storage.Mongo.Crm
{
    public class EventEntity
    {
        public ObjectId Id { get; set; }

        [BsonElement("description")]
        public string Description { get; set; }

        [BsonElement("teacher")]
        public TeacherShortEntity Teacher { get; set; }

        [BsonElement("subject")]
        public SubjectEntity Subject { get; set; }

        [BsonElement("schoolId")]
        public ObjectId SchoolId { get; set; }

        [BsonElement("class")]
        public ClassShortEntity Class { get; set; }

        [BsonElement("date")]
        public DateTime Date { get; set; }

        [BsonElement("from")]
        public TimeSpan From { get; set; }

        [BsonElement("to")]
        public TimeSpan To { get; set; }

        public static implicit operator Event(EventEntity @event)
        {
            return new Event(@event.Id.ToString(),
                @event.SchoolId.ToString(),
                @event.Description,
                @event.Teacher,
                @event.Subject,
                @event.Class,
                @event.Date, @event.From, @event.To);
        }
    }
}