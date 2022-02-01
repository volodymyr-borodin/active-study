using System.Collections.Generic;
using System.Linq;
using ActiveStudy.Domain.Crm;
using ActiveStudy.Domain.Crm.Students;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace ActiveStudy.Storage.Mongo.Crm
{
    public class StudentEntity
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

        [BsonElement("schoolId")]
        public ObjectId SchoolId { get; set; }

        [BsonElement("userId")]
        public string UserId { get; set; }

        [BsonElement("classes")]
        public IList<ClassShortEntity> Classes { get; set; }

        public StudentEntity()
        {
            Classes = new List<ClassShortEntity>();
        }

        public static implicit operator Student(StudentEntity student)
        {
            var classes = student.Classes.Select(c => (ClassShortInfo)c);

            return new Student(student.Id.ToString(), student.FirstName, student.LastName,
                student.Email, student.Phone, student.SchoolId.ToString(), student.UserId, classes);
        }
    }
}