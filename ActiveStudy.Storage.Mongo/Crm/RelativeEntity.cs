using System.Collections.Generic;
using System.Linq;
using ActiveStudy.Domain.Crm;
using ActiveStudy.Domain.Crm.Relatives;
using ActiveStudy.Domain.Crm.Students;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace ActiveStudy.Storage.Mongo.Crm
{
    public class RelativeEntity
    {
        public ObjectId Id { get; set; }

        [BsonElement("firstName")]
        public string FirstName { get; set; }

        [BsonElement("lastName")]
        public string LastName { get; set; }

        [BsonElement("email")]
        public string Email { get; set; }

        [BsonElement("phone")]
        public string Phone { get; set; }

        [BsonElement("studentIds")]
        public IList<string> StudentIds { get; set; }

        public RelativeEntity()
        {
            StudentIds = new List<string>();
        }

        public static implicit operator Relative(RelativeEntity student)
        {
            return new Relative(student.Id.ToString(), student.FirstName, student.LastName,
                student.Email, student.Phone);
        }
    }
}