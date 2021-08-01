using System;
using Microsoft.AspNetCore.Identity;
using MongoDB.Bson.Serialization;
using MongoDB.Driver;

namespace ActiveStudy.AspNetCore.Identity.Mongo
{
    public class IdentityDbContext<TKey>
        where TKey : IEquatable<TKey>
    {
        private static bool isConfigured = false;
        private readonly IMongoDatabase database;

        public IdentityDbContext(IMongoDatabase database)
        {
            this.database = database;

            OnConfigure();
        }

        public IMongoCollection<TEntity> Collection<TEntity>(string name) => database.GetCollection<TEntity>(name);

        protected virtual void OnConfigure()
        {
            if (!isConfigured)
            {
                isConfigured = true;

                BsonClassMap.RegisterClassMap<IdentityRole<TKey>>(cm =>
                {
                    cm.AutoMap();
                    cm.SetIgnoreExtraElements(true);
                });

                BsonClassMap.RegisterClassMap<IdentityRoleClaim<TKey>>(cm =>
                {
                    cm.AutoMap();
                    cm.UnmapMember(m => m.Id);
                    cm.SetIgnoreExtraElements(true);
                });

                BsonClassMap.RegisterClassMap<IdentityUser<TKey>>(cm =>
                {
                    cm.AutoMap();
                    cm.SetIgnoreExtraElements(true);
                });

                BsonClassMap.RegisterClassMap<IdentityUserRole<TKey>>(cm =>
                {
                    cm.AutoMap();
                    cm.SetIgnoreExtraElements(true);
                });

                BsonClassMap.RegisterClassMap<IdentityUserClaim<TKey>>(cm =>
                {
                    cm.AutoMap();
                    cm.UnmapMember(m => m.Id);
                    cm.SetIgnoreExtraElements(true);
                });

                BsonClassMap.RegisterClassMap<IdentityUserLogin<TKey>>(cm =>
                {
                    cm.AutoMap();
                    cm.SetIgnoreExtraElements(true);
                });

                BsonClassMap.RegisterClassMap<IdentityUserToken<TKey>>(cm =>
                {
                    cm.AutoMap();
                    cm.SetIgnoreExtraElements(true);
                });
            }
        }
    }
}
