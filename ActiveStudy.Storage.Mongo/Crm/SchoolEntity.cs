using ActiveStudy.Domain.Crm.Schools;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace ActiveStudy.Storage.Mongo.Crm
{
    public class SchoolEntity
    {
        public ObjectId Id { get; set; }

        [BsonElement("title")]
        public string Title { get; set; }

        [BsonElement("country")]
        public CountryEntity Country { get; set; }

        [BsonElement("owner")]
        public UserEntity Owner { get; set; }

        public static implicit operator School(SchoolEntity school)
        {
            return new School(school.Id.ToString(), school.Title, school.Country, school.Owner);
        }
    }
}