using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ActiveStudy.Domain;
using ActiveStudy.Domain.Crm;
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

        private async Task<EducationPeriod> GetEducationPeriodAsync()
        {
            var educationPeriod = await context.SchedulePeriods.Find(Builders<SchedulePeriodEntity>.Filter.Empty)
                .FirstOrDefaultAsync();

            if (educationPeriod == null)
            {
                return new EducationPeriod(
                    ObjectId.GenerateNewId().ToString(),
                    new DateOnly(DateTime.Today.Year, 1, 1),
                    new DateOnly(DateTime.Today.Year, 12, 31),
                    new Dictionary<int, LessonDuration>
                    {
                        [0] = new LessonDuration(new TimeOnly(8, 30), new TimeOnly(9, 15)),
                        [1] = new LessonDuration(new TimeOnly(9, 30), new TimeOnly(10, 15)),
                        [2] = new LessonDuration(new TimeOnly(10, 30), new TimeOnly(11, 15)),
                        [3] = new LessonDuration(new TimeOnly(11, 30), new TimeOnly(12, 15)),
                        [4] = new LessonDuration(new TimeOnly(12, 30), new TimeOnly(13, 15)),
                        [5] = new LessonDuration(new TimeOnly(13, 30), new TimeOnly(14, 15))
                    });
            }

            return new EducationPeriod(
                educationPeriod.Id.ToString(),
                DateOnly.FromDateTime(educationPeriod.From),
                DateOnly.FromDateTime(educationPeriod.To),
                educationPeriod.Lessons.ToDictionary(
                    x => int.Parse(x.Key),
                    x => new LessonDuration(
                        TimeOnly.FromTimeSpan(x.Value.Start),
                        TimeOnly.FromTimeSpan(x.Value.End))));
        }

        public async Task<ClassSchedule> GetClassScheduleAsync(ClassShortInfo @class)
        {
            var educationPeriod = await GetEducationPeriodAsync();

            var filter = Builders<ScheduleLessonEntity>.Filter.Eq(x => x.PeriodId, ObjectId.Parse(educationPeriod.Id))
                         & Builders<ScheduleLessonEntity>.Filter.Eq(x => x.Class.Id, ObjectId.Parse(@class.Id));

            var lessons = await context.ScheduleLessons
                .Find(filter)
                .ToListAsync();

            var result = lessons.GroupBy(l => l.DayOfWeek)
                .ToDictionary(
                    g => Enum.Parse<DayOfWeek>(g.Key),
                    g => new DaySchedule(g.GroupBy(k => k.Order)
                        .ToDictionary(
                            gg => gg.Key,
                            gg => new ScheduleItem(
                                (ClassShortInfo) gg.First().Class,
                                (TeacherShortInfo) gg.First().Teacher,
                                (Subject) gg.First().Subject))));

            return ClassSchedule.Init(educationPeriod, result);
        }

        public async Task InsertScheduleAsync(EducationPeriod educationPeriod, SchoolClassesSchedule schedule)
        {
            await context.SchedulePeriods.InsertOneAsync(new SchedulePeriodEntity
            {
                Id = ObjectId.Parse(educationPeriod.Id),
                From = educationPeriod.From.ToDateTime(new TimeOnly()),
                To = educationPeriod.To.ToDateTime(new TimeOnly()),
                Lessons = educationPeriod.Lessons
                    .ToDictionary(l => l.Key.ToString(),
                        l => new ScheduleLessonDurationEntity
                        {
                            Start = l.Value.Start.ToTimeSpan(),
                            End = l.Value.End.ToTimeSpan()
                        })
            });

            var entities = new List<ScheduleLessonEntity>();
            foreach (var (_, classSchedule) in schedule)
            {
                foreach (var (dayOfWeek, daySchedule) in classSchedule)
                {
                    foreach (var (order, scheduleItem) in daySchedule)
                    {
                        entities.Add(new ScheduleLessonEntity
                        {
                            Id = ObjectId.GenerateNewId(),
                            PeriodId = ObjectId.Parse(educationPeriod.Id),
                            DayOfWeek = dayOfWeek.ToString(),
                            Order = order,
                            Teacher = (TeacherShortEntity)scheduleItem.Teacher,
                            Class = (ClassShortEntity)scheduleItem.Class,
                            Subject = (SubjectEntity)scheduleItem.Subject
                        });
                    }
                }
            }

            await context.ScheduleLessons.InsertManyAsync(entities);
        }
    }
}