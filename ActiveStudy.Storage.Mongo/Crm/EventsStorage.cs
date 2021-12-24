using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ActiveStudy.Domain.Crm.Scheduler;
using MongoDB.Bson;
using MongoDB.Driver;

namespace ActiveStudy.Storage.Mongo.Crm;

public class EventsStorage : IEventsStorage
{
    private readonly CrmContext context;

    public EventsStorage(CrmContext context)
    {
        this.context = context;
    }

    public async Task<IEnumerable<Event>> GetByClassAsync(string classId, DateOnly from, DateOnly to)
    {
        var filter =
            Builders<EventEntity>.Filter.Eq(entity => entity.Class.Id, ObjectId.Parse(classId))
            & Builders<EventEntity>.Filter.Gte(entity => entity.Date, from.ToDateTime(new TimeOnly()))
            & Builders<EventEntity>.Filter.Lte(entity => entity.Date, to.ToDateTime(new TimeOnly()));

        var entities = await context.Events
            .Find(filter)
            .ToListAsync();

        return entities
            .Select(e => (Event) e)
            .ToList();
    }
}