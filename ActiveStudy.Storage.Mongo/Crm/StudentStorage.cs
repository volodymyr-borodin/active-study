using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ActiveStudy.Domain.Crm.Students;
using MongoDB.Bson;
using MongoDB.Driver;

namespace ActiveStudy.Storage.Mongo.Crm
{
    public class StudentStorage : IStudentStorage
    {
        private readonly CrmContext context;

        public StudentStorage(CrmContext context)
        {
            this.context = context;
        }

        public async Task<IEnumerable<Student>> FindAsync(StudentFilter filter)
        {
            var mongoFilter = Builders<StudentEntity>.Filter.Empty;

            if (!string.IsNullOrEmpty(filter.ClassId))
            {
                mongoFilter &= Builders<StudentEntity>.Filter.ElemMatch(cr => cr.Classes, @class => @class.Id == new ObjectId(filter.ClassId));
            }

            if (!string.IsNullOrEmpty(filter.SchoolId) && ObjectId.TryParse(filter.SchoolId, out var oSchoolId))
            {
                mongoFilter &= Builders<StudentEntity>.Filter.Eq(cr => cr.SchoolId, oSchoolId);
            }

            var entities = await context.Students.Find(mongoFilter).ToListAsync();

            return entities.Select(s => (Student)s).ToList();
        }

        public async Task<Student> GetByIdAsync(string id)
        {
            var filter = Builders<StudentEntity>.Filter.Eq(s => s.Id, new ObjectId(id));

            return await context.Students.Find(filter).FirstOrDefaultAsync();
        }

        public async Task SetUserIdAsync(string id, string userId)
        {
            var idFilter = Builders<StudentEntity>.Filter
                .Eq(s => s.Id, new ObjectId(id));

            var update = Builders<StudentEntity>.Update
                .Set(t => t.UserId, userId);

            await context.Students.UpdateOneAsync(idFilter, update);
        }

        public async Task<string> InsertAsync(Student student)
        {
            var entity = new StudentEntity
            {
                FirstName = student.FirstName,
                LastName = student.LastName,
                Email = student.Email,
                Phone = student.Phone,
                
                Classes = student.Classes.Select(c => (ClassShortEntity)c).ToList(),
                SchoolId = new ObjectId(student.SchoolId)
            };

            await context.Students.InsertOneAsync(entity);

            return entity.Id.ToString();
        }
    }
}