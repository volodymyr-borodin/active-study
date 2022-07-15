using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ActiveStudy.Domain.Crm.Classes;
using MongoDB.Bson;
using MongoDB.Driver;

namespace ActiveStudy.Storage.Mongo.Crm
{
    public class ClassStorage : IClassStorage
    {
        private readonly CrmContext context;

        public ClassStorage(CrmContext context)
        {
            this.context = context;
        }

        public async Task<IEnumerable<Class>> FindAsync(string schoolId)
        {
            var schoolFilter = FilterBuilder.Eq(c => c.SchoolId, new ObjectId(schoolId));
            var entities = await context.Classes.Find(schoolFilter).ToListAsync();

            return entities.Select(e => (Class)e).ToList();
        }

        public async Task<Class> GetByIdAsync(string id)
        {
            var idFilter = FilterBuilder.Eq(c => c.Id, new ObjectId(id));
            var entity = await context.Classes.Find(idFilter).FirstAsync();

            return entity;
        }

        public async Task<string> InsertAsync(Class @class)
        {
            var entity = new ClassEntity
            {
                SchoolId = new ObjectId(@class.SchoolId),
                Grade = @class.Grade,
                Label = @class.Label,
                Teacher = (TeacherShortEntity) @class.Teacher
            };

            await context.Classes.InsertOneAsync(entity);

            return entity.Id.ToString();
        }

        public Task InsertManyAsync(IEnumerable<Class> classes)
        {
            var entities = classes.Select(c => new ClassEntity
            {
                Id = new ObjectId(c.Id),
                SchoolId = new ObjectId(c.SchoolId),
                Grade = c.Grade,
                Label = c.Label,
                Teacher = (TeacherShortEntity) c.Teacher
            });

            return context.Classes.InsertManyAsync(entities);
        }

        public async Task SetTeacherUserIdAsync(string teacherId, string userId)
        {
            var idFilter = Builders<ClassEntity>.Filter
                .Eq(s => s.Teacher.Id, new ObjectId(teacherId));

            var update = Builders<ClassEntity>.Update
                .Set(t => t.Teacher.UserId, userId);

            await context.Classes.UpdateOneAsync(idFilter, update);
        }

        private static FilterDefinitionBuilder<ClassEntity> FilterBuilder => Builders<ClassEntity>.Filter;
    }
}