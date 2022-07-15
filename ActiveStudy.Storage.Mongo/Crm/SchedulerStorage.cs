using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ActiveStudy.Domain;
using ActiveStudy.Domain.Crm;
using ActiveStudy.Domain.Crm.Scheduler;

namespace ActiveStudy.Storage.Mongo.Crm
{
    public class SchedulerStorage : ISchedulerStorage
    {
        private readonly CrmContext context;

        public SchedulerStorage(CrmContext context)
        {
            this.context = context;
        }

        public async Task<Dictionary<ClassShortInfo, Dictionary<DayOfWeek, Dictionary<LessonDuration, ScheduleItem>>>> GetClassBasedSchoolScheduleAsync()
        {
            var c1 = new ClassShortInfo("1", "1-A");
            var c2 = new ClassShortInfo("2", "1-A");

            var t1 = new TeacherShortInfo("1", "Ivan", null);
            var t2 = new TeacherShortInfo("1", "Ivan", null);
            var t3 = new TeacherShortInfo("1", "Ivan", null);

            var s1 = new Subject("Maths", "Maths");
            var s2 = new Subject("Music", "Music");
            var s3 = new Subject("History", "History");

        return new Dictionary<ClassShortInfo, Dictionary<DayOfWeek, Dictionary<LessonDuration, ScheduleItem>>>
        {
            [c1] = new Dictionary<DayOfWeek, Dictionary<LessonDuration, ScheduleItem>>
            {
                [DayOfWeek.Monday] = new Dictionary<LessonDuration, ScheduleItem>
                {
                    [new LessonDuration(new TimeOnly(8, 0), new TimeOnly(9, 30))] = new ScheduleItem(c1, t1, s2),
                    [new LessonDuration(new TimeOnly(9, 45), new TimeOnly(11, 15))] = new ScheduleItem(c1, t2, s1),
                    [new LessonDuration(new TimeOnly(11, 30), new TimeOnly(12, 55))] = new ScheduleItem(c1, t2, s1),
                },
                [DayOfWeek.Thursday] = new Dictionary<LessonDuration, ScheduleItem>
                {
                    [new LessonDuration(new TimeOnly(8, 0), new TimeOnly(9, 30))] = new ScheduleItem(c1, t1, s2),
                    [new LessonDuration(new TimeOnly(9, 45), new TimeOnly(11, 15))] = new ScheduleItem(c1, t1, s2),
                    [new LessonDuration(new TimeOnly(11, 30), new TimeOnly(12, 55))] = new ScheduleItem(c1, t2, s1),
                }
            },
            [c2] = new Dictionary<DayOfWeek, Dictionary<LessonDuration, ScheduleItem>>
            {
                [DayOfWeek.Monday] = new Dictionary<LessonDuration, ScheduleItem>
                {
                    [new LessonDuration(new TimeOnly(8, 0), new TimeOnly(9, 30))] = new ScheduleItem(c1, t1, s2),
                    [new LessonDuration(new TimeOnly(9, 45), new TimeOnly(11, 15))] = new ScheduleItem(c1, t2, s1),
                    [new LessonDuration(new TimeOnly(11, 30), new TimeOnly(12, 55))] = new ScheduleItem(c1, t2, s1),
                },
                [DayOfWeek.Thursday] = new Dictionary<LessonDuration, ScheduleItem>
                {
                    [new LessonDuration(new TimeOnly(8, 0), new TimeOnly(9, 30))] = new ScheduleItem(c1, t1, s2),
                    [new LessonDuration(new TimeOnly(9, 45), new TimeOnly(11, 15))] = new ScheduleItem(c1, t1, s2),
                    [new LessonDuration(new TimeOnly(11, 30), new TimeOnly(12, 55))] = new ScheduleItem(c1, t2, s1),
                }
            }
        };
        }

        public async Task<Dictionary<DayOfWeek, Dictionary<LessonDuration, ScheduleItem>>> GetClassScheduleAsync(ClassShortInfo classShortInfo)
        {
            return (await GetClassBasedSchoolScheduleAsync()).Values.First();
        }
    }
}