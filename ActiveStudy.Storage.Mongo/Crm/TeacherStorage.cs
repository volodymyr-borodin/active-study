using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ActiveStudy.Domain.Crm.Teachers;
using MongoDB.Bson;
using MongoDB.Driver;

namespace ActiveStudy.Storage.Mongo.Crm
{
    public class TeacherStorage : ITeacherStorage
    {
        private readonly CrmContext context;

        public TeacherStorage(CrmContext context)
        {
            this.context = context;
        }

        public async Task<IEnumerable<Teacher>> FindAsync(string schoolId)
        {
            var schoolFilter = FilterBuilder.Eq(t => t.SchoolId, new ObjectId(schoolId));
            var entities = await context.Teachers.Find(schoolFilter).ToListAsync();

            return entities.Select(s => (Teacher)s).ToList();
        }

        public async Task<Teacher> GetByIdAsync(string id)
        {
            var idFilter = FilterBuilder.Eq(cr => cr.Id, new ObjectId(id));

            return await context.Teachers.Find(idFilter).FirstAsync();
        }

        public async Task<string> InsertAsync(Teacher teacher)
        {
            var entity = new TeacherEntity
            {
                FirstName = teacher.FirstName,
                LastName = teacher.LastName,
                MiddleName = teacher.MiddleName,
                Email = teacher.Email,
                SchoolId = new ObjectId(teacher.SchoolId),
                Subjects = teacher.Subjects.Select(s => (SubjectEntity)s).ToList(),
                UserId = teacher.UserId
            };

            await context.Teachers.InsertOneAsync(entity);

            return entity.Id.ToString();
        }

        public async Task InsertManyAsync(IEnumerable<Teacher> teachers)
        {
            var entities = teachers.Select(t => new TeacherEntity
            {
                Id = new ObjectId(t.Id),
                FirstName = t.FirstName,
                LastName = t.LastName,
                MiddleName = t.MiddleName,
                Email = t.Email,
                SchoolId = new ObjectId(t.SchoolId),
                Subjects = t.Subjects.Select(s => (SubjectEntity)s).ToList(),
                UserId = t.UserId
            }).ToList();

            await context.Teachers.InsertManyAsync(entities);
        }

        public async Task DeleteAsync(Teacher teacher)
        {
            var idFilter = Builders<TeacherEntity>.Filter
                .Eq(s => s.Id, new ObjectId(teacher.Id));

            await context.Teachers.DeleteOneAsync(idFilter);
        }

        public async Task SetUserIdAsync(string id, string userId)
        {
            var idFilter = Builders<TeacherEntity>.Filter
                .Eq(s => s.Id, new ObjectId(id));

            var update = Builders<TeacherEntity>.Update
                .Set(t => t.UserId, userId);

            await context.Teachers.UpdateOneAsync(idFilter, update);
        }

        private static FilterDefinitionBuilder<TeacherEntity> FilterBuilder => Builders<TeacherEntity>.Filter;
    }
}