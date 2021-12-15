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

        public static explicit operator TeacherShortInfo(TeacherShortEntity entity)
        {
            return entity == null
                ? null
                : new TeacherShortInfo(entity.Id.ToString(), entity.FullName, entity.UserId);
        }

        public static explicit operator TeacherShortEntity(TeacherShortInfo info)
        {
            if (info == null)
            {
                return null;
            }

            return new TeacherShortEntity
            {
                Id = new ObjectId(info.Id),
                FullName = info.FullName,
                UserId = info.UserId
            };
        }
    }
}