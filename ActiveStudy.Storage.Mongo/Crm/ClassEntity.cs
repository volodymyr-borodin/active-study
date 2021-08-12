using ActiveStudy.Domain.Crm.Classes;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace ActiveStudy.Storage.Mongo.Crm
{
    public class ClassEntity
    {
        public ObjectId Id { get; set; }

        [BsonElement("grade")]
        public int? Grade { get; set; }

        [BsonElement("label")]
        public string Label { get; set; }
        
        [BsonElement("teacher")]
        public TeacherShortEntity Teacher { get; set; }

        [BsonElement("schoolId")]
        public ObjectId SchoolId { get; set; }

        public static implicit operator Class(ClassEntity entity)
        {
            return new Class(entity.Id.ToString(), entity.Grade, entity.Label, entity.Teacher, entity.SchoolId.ToString());
        }
    }
}