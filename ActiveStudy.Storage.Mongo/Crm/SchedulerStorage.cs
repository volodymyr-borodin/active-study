using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ActiveStudy.Domain.Crm.Scheduler;
using MongoDB.Bson;
using MongoDB.Driver;

namespace ActiveStudy.Storage.Mongo.Crm
{
    public class SchedulerStorage : ISchedulerStorage
    {
        private readonly CrmContext context;

        public SchedulerStorage(CrmContext context)
        {
            this.context = context;
        }

        public async Task CreateAsync(Event @event)
        {
            var entity = new EventEntity
            {
                Description = @event.Description,
                Teacher = (TeacherShortEntity) @event.Teacher,
                Subject = (SubjectEntity) @event.Subject,
                SchoolId = ObjectId.Parse(@event.SchoolId),
                Class = (ClassShortEntity) @event.Class,
                Date = @event.Date.ToDateTime(new TimeOnly()),
                From = @event.From.ToTimeSpan(),
                To = @event.To.ToTimeSpan()
            };
            
            await context.Events.InsertOneAsync(entity);
        }

        public async Task<Schedule> GetByClassAsync(string classId, DateTime from, DateTime to)
        {
            var filter =
                Builders<EventEntity>.Filter.Eq(entity => entity.Class.Id, ObjectId.Parse(classId))
                & Builders<EventEntity>.Filter.Gte(entity => entity.Date, from)
                & Builders<EventEntity>.Filter.Lte(entity => entity.Date, to);

            var entities = await context.Events
                .Find(filter)
                .ToListAsync();

            var dict = DaysRange(from, to)
                .ToDictionary(DateOnly.FromDateTime, day => (IReadOnlyCollection<Event>) entities
                    .Where(e => e.Date == day)
                    .OrderBy(e => e.From)
                    .Select(e => (Event) e));

            return new Schedule(dict);
        }

        public async Task<Schedule> GetByTeacherAsync(string teacherId, DateTime from, DateTime to)
        {
            var filter =
                Builders<EventEntity>.Filter.Eq(entity => entity.Teacher.Id, ObjectId.Parse(teacherId))
                & Builders<EventEntity>.Filter.Gte(entity => entity.Date, from)
                & Builders<EventEntity>.Filter.Lte(entity => entity.Date, to);

            var entities = await context.Events
                .Find(filter)
                .ToListAsync();

            var dict = DaysRange(from, to)
                .ToDictionary(DateOnly.FromDateTime, day => (IReadOnlyCollection<Event>) entities
                    .Where(e => e.Date == day)
                    .OrderBy(e => e.From)
                    .Select(e => (Event) e));

            return new Schedule(dict);
        }

        private static IEnumerable<DateTime> DaysRange(DateTime from, DateTime to)
        {
            while (from < to)
            {
                yield return from;
                from = from.AddDays(1);
            }
        }
    }
}