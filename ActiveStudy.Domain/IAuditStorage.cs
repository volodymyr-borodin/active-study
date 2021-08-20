using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ActiveStudy.Domain
{
    public interface IAuditStorage
    {
        public Task LogAsync(AuditItem item);
        public Task<IEnumerable<AuditItem>> SearchAnyAsync(IEnumerable<AuditEntity> entities);
    }

    public enum EntityType
    {
        School,
        Student,
        Teacher,
        Class
    }

    public class AuditEntity
    {
        public string Id { get; }
        public EntityType EntityType { get; }

        public AuditEntity(string id, EntityType entityType)
        {
            Id = id;
            EntityType = entityType;
        }
    }

    public class AuditItem
    {
        public string Message { get; }
        public DateTime Time { get; }
        public User User { get; }
        
        public IEnumerable<AuditEntity> Entities { get; }

        public AuditItem(string message, User user, IEnumerable<AuditEntity> entities)
        {
            Message = message;
            User = user;
            Entities = entities;

            Time = DateTime.UtcNow;
        }
    }
}