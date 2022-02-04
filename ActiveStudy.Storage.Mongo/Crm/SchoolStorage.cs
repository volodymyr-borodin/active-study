using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ActiveStudy.Domain;
using ActiveStudy.Domain.Crm.Schools;
using MongoDB.Bson;
using MongoDB.Driver;

namespace ActiveStudy.Storage.Mongo.Crm
{
    public class SchoolStorage : ISchoolStorage
    {
        private readonly CrmContext context;

        public SchoolStorage(CrmContext context)
        {
            this.context = context;
        }

        public async Task<string> CreateAsync(School school)
        {
            var entity = new SchoolEntity
            {
                Title = school.Title,
                Description = school.Description,
                Country = school.Country,
                Owner = school.Owner
            };

            if (ObjectId.TryParse(school.Id, out var oid))
            {
                entity.Id = oid;
            }

            await context.Schools.InsertOneAsync(entity);

            return entity.Id.ToString();
        }

        public async Task<IEnumerable<Subject>> GetSubjectsAsync(string id, IEnumerable<string> subjectIds = null)
        {
            var filter = Builders<SchoolSubjectEntity>.Filter.Eq(subject => subject.SchoolId, id);
            if (subjectIds != null)
            {
                filter &= Builders<SchoolSubjectEntity>.Filter.In(cr => cr.Id, subjectIds.Select(sid => new ObjectId(sid)));
            }

            var entities = await context.SchoolSubjects
                .Find(filter)
                .ToListAsync();

            return entities
                .Select(e => new Subject(e.Id.ToString(), e.Title))
                .ToList();
        }

        public async Task InsertSubjectsAsync(string id, IEnumerable<Subject> subjects)
        {
            var entities = subjects.Select(s => new SchoolSubjectEntity
            {
                Id = s.Id == null
                    ? ObjectId.GenerateNewId()
                    : ObjectId.Parse(s.Id),
                SchoolId = id,
                Title = s.Title
            });

            await context.SchoolSubjects.InsertManyAsync(entities);
        }

        public async Task<IEnumerable<School>> SearchByIdsAsync(IEnumerable<string> ids)
        {
            var idsFilter = FilterBuilder.In(cr => cr.Id, ids.Select(id => new ObjectId(id)));

            var entities = await context.Schools.Find(idsFilter).ToListAsync();

            return entities.Select(e => (School) e).ToList();
        }

        public async Task<School> GetByIdAsync(string id)
        {
            var idFilter = FilterBuilder.Eq(cr => cr.Id, new ObjectId(id));

            return await context.Schools.Find(idFilter).FirstAsync();
        }

        private static FilterDefinitionBuilder<SchoolEntity> FilterBuilder => Builders<SchoolEntity>.Filter;
    }
}