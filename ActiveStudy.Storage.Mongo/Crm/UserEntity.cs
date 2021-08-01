using ActiveStudy.Domain;
using MongoDB.Bson.Serialization.Attributes;

namespace ActiveStudy.Storage.Mongo.Crm
{
    public class UserEntity
    {
        [BsonElement("userId")]
        public string UserId { get; set; }

        [BsonElement("fullName")]
        public string FullName { get; set; }

        public static implicit operator User(UserEntity entity)
        {
            return new User(entity.UserId, entity.FullName);
        }

        public static implicit operator UserEntity(User info)
        {
            return new UserEntity
            {
                UserId = info.Id,
                FullName = info.FullName
            };
        }
    }
}