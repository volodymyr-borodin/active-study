using ActiveStudy.Domain;
using MongoDB.Bson.Serialization.Attributes;

namespace ActiveStudy.Storage.Mongo
{
    public class SubjectEntity
    {
        [BsonElement("_id")]
        public string Id { get; set; }

        [BsonElement("title")]
        public string Title { get; set; }

        public static implicit operator Subject(SubjectEntity entity)
        {
            return new Subject(entity.Id, entity.Title);
        }

        public static implicit operator SubjectEntity(Subject entity)
        {
            return new SubjectEntity
            {
                Id = entity.Id,
                Title = entity.Title
            };
        }
    }
}