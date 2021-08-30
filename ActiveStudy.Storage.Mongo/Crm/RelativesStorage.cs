using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ActiveStudy.Domain.Crm.Relatives;
using MongoDB.Bson;
using MongoDB.Driver;

namespace ActiveStudy.Storage.Mongo.Crm
{
    public class RelativesStorage : IRelativesStorage
    {
        private readonly CrmContext context;

        public RelativesStorage(CrmContext context)
        {
            this.context = context;
        }

        public async Task<string> InsertAsync(Relative relative)
        {
            var entity = new RelativeEntity
            {
                FirstName = relative.FirstName,
                LastName = relative.LastName,
                Email = relative.Email,
                Phone = relative.Phone,
                StudentIds = new List<string>()
            };
            
            await context.Relatives.InsertOneAsync(entity);

            return entity.Id.ToString();
        }

        public async Task AddStudentAsync(string relativeId, string studentId)
        {
            var filter = Builders<RelativeEntity>.Filter.Eq(r => r.Id, new ObjectId(relativeId));
            var update = Builders<RelativeEntity>.Update
                .Push(r => r.StudentIds, studentId);

            await context.Relatives.UpdateOneAsync(filter, update);
        }

        public async Task<IEnumerable<Relative>> SearchAsync(string studentId)
        {
            var filter = Builders<RelativeEntity>.Filter.AnyEq(r => r.StudentIds, studentId);

            var entities = await context.Relatives.Find(filter).ToListAsync();

            return entities.Select(e => (Relative) e).ToList();
        }

        public async Task<IDictionary<string, IEnumerable<Relative>>> SearchAsync(IEnumerable<string> studentIds)
        {
            var filter = Builders<RelativeEntity>.Filter.AnyIn(r => r.StudentIds, studentIds);

            var entities = await context.Relatives.Find(filter).ToListAsync();

            return studentIds
                .ToDictionary(s => s,
                    s => entities
                        .Where(e => e.StudentIds.Contains(s))
                        .Select(e => (Relative) e));
        }
    }
}