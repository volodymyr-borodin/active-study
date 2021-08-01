using ActiveStudy.Domain.Crm;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace ActiveStudy.Storage.Mongo.Crm
{
    public class ClassShortEntity
    {
        public ObjectId Id { get; set; }

        [BsonElement("title")]
        public string Title { get; set; }

        public static implicit operator ClassShortInfo(ClassShortEntity entity)
        {
            if (entity == null)
            {
                return null;
            }

            return new ClassShortInfo(entity.Id.ToString(), entity.Title);
        }

        public static implicit operator ClassShortEntity(ClassShortInfo info)
        {
            if (info == null)
            {
                return null;
            }

            return new ClassShortEntity
            {
                Id = new ObjectId(info.Id),
                Title = info.Title
            };
        }
    }
}