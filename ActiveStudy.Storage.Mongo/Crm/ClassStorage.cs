using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ActiveStudy.Domain;
using ActiveStudy.Domain.Crm;
using ActiveStudy.Domain.Crm.Classes;
using ActiveStudy.Domain.Crm.Classes.ScheduleTemplate;
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
                Teacher = (TeacherShortEntity) @class.Teacher
            };

            await context.Classes.InsertOneAsync(entity);

            return entity.Id.ToString();
        }

        public Task InsertManyAsync(IEnumerable<Class> classes)
        {
            var entities = classes.Select(c => new ClassEntity
            {
                Id = new ObjectId(c.Id),
                SchoolId = new ObjectId(c.SchoolId),
                Grade = c.Grade,
                Label = c.Label,
                Teacher = (TeacherShortEntity) c.Teacher
            });

            return context.Classes.InsertManyAsync(entities);
        }

        public async Task SetTeacherUserIdAsync(string teacherId, string userId)
        {
            var idFilter = Builders<ClassEntity>.Filter
                .Eq(s => s.Teacher.Id, new ObjectId(teacherId));

            var update = Builders<ClassEntity>.Update
                .Set(t => t.Teacher.UserId, userId);

            await context.Classes.UpdateOneAsync(idFilter, update);
        }

        public async Task SaveScheduleTemplateAsync(string classId, ClassScheduleTemplate schedule)
        {
            var filter = Builders<ScheduleTemplateEntity>.Filter.Eq(s => s.ClassId, ObjectId.Parse(classId));
            if (await context.ScheduleTemplates.Find(filter).AnyAsync())
            {
                await UpdateAsync(classId, schedule);
            }
            else
            {
                await InsertAsync(classId, schedule);
            }
        }

        private async Task InsertAsync(string classId, ClassScheduleTemplate schedule)
        {
            var schedulePeriods = new List<ScheduleTemplatePeriodEntity>();
            foreach (var p in schedule.Periods)
            {
                var period = new ScheduleTemplatePeriodEntity
                {
                    Start = p.Start.ToTimeSpan(),
                    End = p.End.ToTimeSpan(),
                    Lessons = new Dictionary<string, ScheduleTemplateLessonEntity>()
                };

                foreach (var (dayOfWeek, lesson) in p.Lessons)
                {
                    if (lesson.Subject == null || lesson.Teacher == null)
                    {
                        continue;
                    }

                    period.Lessons[dayOfWeek.ToString()] = new ScheduleTemplateLessonEntity
                    {
                        Teacher = (TeacherShortEntity) lesson.Teacher,
                        Subject = (SubjectEntity) lesson.Subject
                    };
                }
                schedulePeriods.Add(period);
            }
            
            var entity = new ScheduleTemplateEntity
            {
                ClassId = ObjectId.Parse(classId),
                EffectiveFrom = schedule.EffectiveFrom.ToDateTime(new TimeOnly()),
                EffectiveTo = schedule.EffectiveTo.ToDateTime(new TimeOnly()),
                Periods = schedulePeriods
            };

            await context.ScheduleTemplates.InsertOneAsync(entity);
        }

        private async Task UpdateAsync(string classId, ClassScheduleTemplate schedule)
        {
            var schedulePeriods = new List<ScheduleTemplatePeriodEntity>();
            foreach (var p in schedule.Periods)
            {
                var period = new ScheduleTemplatePeriodEntity
                {
                    Start = p.Start.ToTimeSpan(),
                    End = p.End.ToTimeSpan(),
                    Lessons = new Dictionary<string, ScheduleTemplateLessonEntity>()
                };

                foreach (var (dayOfWeek, lesson) in p.Lessons)
                {
                    if (lesson.Subject == null || lesson.Teacher == null)
                    {
                        continue;
                    }

                    period.Lessons[dayOfWeek.ToString()] = new ScheduleTemplateLessonEntity
                    {
                        Teacher = (TeacherShortEntity) lesson.Teacher,
                        Subject = (SubjectEntity) lesson.Subject
                    };
                }
                schedulePeriods.Add(period);
            }

            var filter = Builders<ScheduleTemplateEntity>.Filter.Eq(s => s.ClassId, ObjectId.Parse(classId));
            var update = Builders<ScheduleTemplateEntity>.Update
                .Set(s => s.EffectiveFrom, schedule.EffectiveFrom.ToDateTime(new TimeOnly()))
                .Set(s => s.EffectiveTo, schedule.EffectiveTo.ToDateTime(new TimeOnly()))
                .Set(s => s.Periods, schedulePeriods);

            await context.ScheduleTemplates.UpdateOneAsync(filter, update);
        }

        public async Task<ClassScheduleTemplate> GetScheduleTemplateAsync(string classId)
        {
            var filter = Builders<ScheduleTemplateEntity>.Filter.Eq(s => s.ClassId, ObjectId.Parse(classId));
            var entity = await context.ScheduleTemplates.Find(filter).FirstOrDefaultAsync();
            if (entity == null)
            {
                return null;
            }

            var @class = await GetByIdAsync(classId);
            var schedulePeriods = new List<SchedulePeriod>();
            foreach (var periodEntity in entity.Periods)
            {
                var lessons = new Dictionary<DayOfWeek, ScheduleTemplateLesson>
                {
                    [DayOfWeek.Monday] = null,
                    [DayOfWeek.Tuesday] = null,
                    [DayOfWeek.Wednesday] = null,
                    [DayOfWeek.Thursday] = null,
                    [DayOfWeek.Friday] = null,
                    [DayOfWeek.Saturday] = null,
                    [DayOfWeek.Sunday] = null
                };
                foreach (var (dayOfWeek, lesson) in periodEntity.Lessons)
                {
                    lessons[Enum.Parse<DayOfWeek>(dayOfWeek)] = new ScheduleTemplateLesson(
                        (ClassShortInfo) @class,
                        (TeacherShortInfo) lesson.Teacher,
                        (Subject) lesson.Subject);
                }

                schedulePeriods.Add(new SchedulePeriod(TimeOnly.FromTimeSpan(periodEntity.Start), TimeOnly.FromTimeSpan(periodEntity.End), lessons));
            }

            var (scheduleTemplate, _) = ClassScheduleTemplate.New(DateOnly.FromDateTime(entity.EffectiveFrom), DateOnly.FromDateTime(entity.EffectiveTo), schedulePeriods);

            return scheduleTemplate;
        }

        private static FilterDefinitionBuilder<ClassEntity> FilterBuilder => Builders<ClassEntity>.Filter;
    }
}