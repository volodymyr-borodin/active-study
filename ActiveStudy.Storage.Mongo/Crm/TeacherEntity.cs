using System.Collections.Generic;
using System.Linq;
using ActiveStudy.Domain;
using ActiveStudy.Domain.Crm.Teachers;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace ActiveStudy.Storage.Mongo.Crm
{
    public class TeacherEntity
    {
        public ObjectId Id { get; set; }

        [BsonElement("schoolId")]
        public ObjectId SchoolId { get; set; }
        
        [BsonElement("userId")]
        public string UserId { get; set; }

        [BsonElement("firstName")]
        public string FirstName { get; set; }

        [BsonElement("lastName")]
        public string LastName { get; set; }

        [BsonElement("email")]
        public string Email { get; set; }

        [BsonElement("subjects")]
        public List<SubjectEntity> Subjects { get; set; }

        public TeacherEntity()
        {
            Subjects = new List<SubjectEntity>();
        }

        public static implicit operator Teacher(TeacherEntity entity)
        {
            var subjects = entity.Subjects.Select(s => (Subject)s);

            return new Teacher(entity.Id.ToString(), entity.FirstName, entity.LastName,
                entity.Email, subjects, entity.SchoolId.ToString(), entity.UserId);
        }
    }
}