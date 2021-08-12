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
                Teacher = @class.Teacher
            };

            await context.Classes.InsertOneAsync(entity);

            return entity.Id.ToString();
        }

        private static FilterDefinitionBuilder<ClassEntity> FilterBuilder => Builders<ClassEntity>.Filter;
    }
}