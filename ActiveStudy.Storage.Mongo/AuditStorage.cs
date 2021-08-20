using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ActiveStudy.Domain;
using MongoDB.Driver;

namespace ActiveStudy.Storage.Mongo
{
    public class AuditStorage : IAuditStorage
    {
        private readonly CommonContext context;

        public AuditStorage(CommonContext context)
        {
            this.context = context;
        }
        
        public async Task LogAsync(AuditItem item)
        {
            await context.Audit.InsertOneAsync(new AuditItemEntity
            {
                Message = item.Message,
                Time = item.Time,
                User = item.User,
                Entities = item.Entities
                    .Select(e => new AuditObjectEntity
                    {
                        Id = e.Id,
                        EntityType = e.EntityType
                    })
            });
        }

        public async Task<IEnumerable<AuditItem>> SearchAnyAsync(IEnumerable<AuditEntity> entities)
        {
            var filters = entities
                .Select(e => Builders<AuditItemEntity>.Filter
                    .ElemMatch(
                        a => a.Entities,
                        a => a.Id == e.Id && a.EntityType == e.EntityType));

            var result = await context.Audit
                .Find(Builders<AuditItemEntity>.Filter.Or(filters))
                .ToListAsync();

            return result
                .Select(e => new AuditItem(e.Message, e.User, entities
                    .Select(e => new AuditEntity(e.Id, e.EntityType))))
                .ToList();
        }
    }
}