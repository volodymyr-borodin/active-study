using ActiveStudy.Domain;
using MongoDB.Bson.Serialization.Attributes;

namespace ActiveStudy.Storage.Mongo
{
    public class CountryEntity
    {
        [BsonElement("name")]
        public string Name { get; set; }
        
        [BsonElement("code")]
        public string Code { get; set; }

        public static implicit operator Country(CountryEntity entity)
        {
            return new Country(entity.Name, entity.Code);
        }

        public static implicit operator CountryEntity(Country entity)
        {
            return new CountryEntity
            {
                Name = entity.Name,
                Code = entity.Code
            };
        }
    }
}