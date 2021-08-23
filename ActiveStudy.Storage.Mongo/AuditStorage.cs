using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ActiveStudy.Domain;
using MongoDB.Bson;
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
            var dataElements = item.Data
                .Select(kv => new BsonElement(kv.Key, kv.Value));
            
            await context.Audit.InsertOneAsync(new AuditItemEntity
            {
                ActionId = item.ActionId,
                Time = item.Time,
                User = item.User,
                Data = new BsonDocument(dataElements)
            });
        }

        public async Task<IEnumerable<AuditItem>> SearchAnyAsync(IDictionary<string, string> filter)
        {
            if (filter == null || filter.Count == 0)
            {
                throw new ArgumentException("Filter can not be empty");
            }
            
            var filterDocument = new BsonDocument(filter.Select(kv => new BsonElement("data." + kv.Key, kv.Value)));

            var result = await context.Audit
                .Find(filterDocument)
                .ToListAsync();

            return result
                .Select(r => new AuditItem(r.ActionId,
                    r.Data.Elements.ToDictionary(e => e.Name, e => e.Value.AsString),
                    r.User))
                .ToList();
        }
    }
}