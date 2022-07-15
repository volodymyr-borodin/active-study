using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace ActiveStudy.Storage.Mongo.Crm;

public class ScheduleLessonEntity
{
    public ObjectId Id { get; set; }

    [BsonElement("periodId")]
    public ObjectId PeriodId { get; set; }

    [BsonElement("dayOfWeek")]
    public string DayOfWeek { get; set; }

    [BsonElement("order")]
    public int Order { get; set; }

    [BsonElement("teacher")]
    public TeacherShortEntity Teacher { get; set; }

    [BsonElement("class")]
    public ClassShortEntity Class { get; set; }

    [BsonElement("subject")]
    public SubjectEntity Subject { get; set; }
}