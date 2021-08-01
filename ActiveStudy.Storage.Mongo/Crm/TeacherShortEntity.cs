using ActiveStudy.Domain.Crm;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace ActiveStudy.Storage.Mongo.Crm
{
    public class TeacherShortEntity
    {
        public ObjectId Id { get; set; }

        [BsonElement("fullName")]
        public string FullName { get; set; }
        
        [BsonElement("userId")]
        public string UserId { get; set; }

        public static implicit operator TeacherShortInfo(TeacherShortEntity entity)
        {
            return new TeacherShortInfo(entity.Id.ToString(), entity.FullName, entity.UserId);
        }

        public static implicit operator TeacherShortEntity(TeacherShortInfo info)
        {
            return new TeacherShortEntity
            {
                Id = new ObjectId(info.Id),
                FullName = info.FullName,
                UserId = info.UserId
            };
        }
    }
}