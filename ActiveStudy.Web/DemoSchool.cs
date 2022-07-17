using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ActiveStudy.Domain;
using ActiveStudy.Domain.Crm;
using ActiveStudy.Domain.Crm.Classes;
using ActiveStudy.Domain.Crm.Scheduler;
using ActiveStudy.Domain.Crm.Schools;
using ActiveStudy.Domain.Crm.Teachers;
using MongoDB.Bson;

namespace ActiveStudy.Web;

public class DemoSchool
{
    private readonly ISchoolStorage schoolStorage;
    private readonly ITeacherStorage teacherStorage;
    private readonly IClassStorage classStorage;
    private readonly ISchedulerStorage schedulerStorage;

    public DemoSchool(
        ISchoolStorage schoolStorage,
        ITeacherStorage teacherStorage,
        IClassStorage classStorage,
        ISchedulerStorage schedulerStorage)
    {
        this.schoolStorage = schoolStorage;
        this.teacherStorage = teacherStorage;
        this.classStorage = classStorage;
        this.schedulerStorage = schedulerStorage;
    }

    public async Task<School> InitAsync(User owner)
    {
        // init school
        var school = new School(ObjectId.GenerateNewId().ToString(), $"Demo {Guid.NewGuid()}", string.Empty, new Country("Україна", "UA"), owner);
        await schoolStorage.CreateAsync(school);

        // init subjects
        var geography = new Subject(ObjectId.GenerateNewId().ToString(), "Geography");
        var history = new Subject(ObjectId.GenerateNewId().ToString(), "History");
        var literacy = new Subject(ObjectId.GenerateNewId().ToString(), "Literacy");
        var music = new Subject(ObjectId.GenerateNewId().ToString(), "Music");
        var mathematics = new Subject(ObjectId.GenerateNewId().ToString(), "Mathematics");
        await schoolStorage.InsertSubjectsAsync(school.Id, new[]
        {
            geography, history, literacy, music, mathematics
        });

        // init teachers
        var t1 = new Teacher(ObjectId.GenerateNewId().ToString(), "Анастасія", "Боднаренко", "Боднаренко",
            "a.bodnarenko@yopmail.com", new[] { mathematics }, school.Id, null);
        var t2 = new Teacher(ObjectId.GenerateNewId().ToString(), "Ярослав", "Романченко", "Іванович",
            "y.romanchenko@yopmail.com", new[] { history }, school.Id, null);
        var t3 = new Teacher(ObjectId.GenerateNewId().ToString(), "Ольга", "Пономарчук", "Євгеніївна",
            "o.ponomarchuk@yopmail.com", new[] { literacy }, school.Id, null);
        var t4 = new Teacher(ObjectId.GenerateNewId().ToString(), "Валерія", "Броваренко", "Олександрівна",
            "v.brovarenko@yopmail.com", new[] { music }, school.Id, null);
        var t5 = new Teacher(ObjectId.GenerateNewId().ToString(), "Данил", "Іванченко", "Федорович",
            "d.ivanchenko@yopmail.com", new[] { geography }, school.Id, null);
        var t6 = new Teacher(ObjectId.GenerateNewId().ToString(), "Алла", "Василенко", "Володимирівна",
            "a.tarasovych@yopmail.com", new[] { mathematics }, school.Id, null);
        var t7 = new Teacher(ObjectId.GenerateNewId().ToString(), "Михайло", "Гнатюк", "Тарасович",
            "m.hnatyuk@yopmail.com", new[] { literacy }, school.Id, null);

        await teacherStorage.InsertManyAsync(new[]
        {
            t1, t2, t3, t4, t5, t6, t7
        });

        // init classes
        var class1A = new Class(ObjectId.GenerateNewId().ToString(), 1, "A", (TeacherShortInfo) t1, school.Id);
        var class1B = new Class(ObjectId.GenerateNewId().ToString(), 1, "B", (TeacherShortInfo) t1, school.Id);
        var class2A = new Class(ObjectId.GenerateNewId().ToString(), 2, "A", (TeacherShortInfo) t2, school.Id);
        var class2B = new Class(ObjectId.GenerateNewId().ToString(), 2, "B", null, school.Id);
        var class3A = new Class(ObjectId.GenerateNewId().ToString(), 3, "A", null, school.Id);
        var class3B = new Class(ObjectId.GenerateNewId().ToString(), 3, "B", (TeacherShortInfo) t3, school.Id);
        var class4A = new Class(ObjectId.GenerateNewId().ToString(), 4, "A", (TeacherShortInfo) t4, school.Id);
        var class4B = new Class(ObjectId.GenerateNewId().ToString(), 5, "B", (TeacherShortInfo) t5, school.Id);

        await classStorage.InsertManyAsync(new[]
        {
            class1A, class1B, class2A, class2B, class3A, class3B, class4A, class4B
        });

        var educationPeriod = new EducationPeriod(
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

        var class1ASchedule = new ClassSchedule( educationPeriod, new Dictionary<DayOfWeek, DaySchedule>
        {
            [DayOfWeek.Monday] = new DaySchedule(new Dictionary<int, ScheduleItem>
            {
                [0] = new ScheduleItem((ClassShortInfo) class1A, (TeacherShortInfo) t1, mathematics),
                [1] = new ScheduleItem((ClassShortInfo) class1A, (TeacherShortInfo) t1, mathematics),
                [2] = new ScheduleItem((ClassShortInfo) class1A, (TeacherShortInfo) t4, music)
            }),
            [DayOfWeek.Tuesday] = new DaySchedule(new Dictionary<int, ScheduleItem>
            {
                [1] = new ScheduleItem((ClassShortInfo) class1A, (TeacherShortInfo) t7, literacy),
                [2] = new ScheduleItem((ClassShortInfo) class1A, (TeacherShortInfo) t7, literacy)
            }),
            [DayOfWeek.Wednesday] = new DaySchedule(new Dictionary<int, ScheduleItem>
            {
                [0] = new ScheduleItem((ClassShortInfo) class1A, (TeacherShortInfo) t1, mathematics),
                [1] = new ScheduleItem((ClassShortInfo) class1A, (TeacherShortInfo) t2, history)
            }),
            [DayOfWeek.Thursday] = new DaySchedule(new Dictionary<int, ScheduleItem>
            {
                [0] = new ScheduleItem((ClassShortInfo) class1A, (TeacherShortInfo) t2, history),
                [1] = new ScheduleItem((ClassShortInfo) class1A, (TeacherShortInfo) t1, mathematics),
                [2] = new ScheduleItem((ClassShortInfo) class1A, (TeacherShortInfo) t1, mathematics)
            }),
            [DayOfWeek.Friday] = new DaySchedule(new Dictionary<int, ScheduleItem>
            {
                [0] = new ScheduleItem((ClassShortInfo) class1A, (TeacherShortInfo) t7, literacy),
                [1] = new ScheduleItem((ClassShortInfo) class1A, (TeacherShortInfo) t1, mathematics)
            })
        });

        var class1BSchedule = new ClassSchedule(educationPeriod, new Dictionary<DayOfWeek, DaySchedule>
        {
            [DayOfWeek.Monday] = new DaySchedule(new Dictionary<int, ScheduleItem>
            {
                [0] = new ScheduleItem((ClassShortInfo) class1B, (TeacherShortInfo) t1, mathematics),
                [1] = new ScheduleItem((ClassShortInfo) class1B, (TeacherShortInfo) t1, mathematics),
                [2] = new ScheduleItem((ClassShortInfo) class1B, (TeacherShortInfo) t4, music)
            }),
            [DayOfWeek.Tuesday] = new DaySchedule(new Dictionary<int, ScheduleItem>
            {
                [1] = new ScheduleItem((ClassShortInfo) class1B, (TeacherShortInfo) t7, literacy),
                [2] = new ScheduleItem((ClassShortInfo) class1B, (TeacherShortInfo) t7, literacy)
            }),
            [DayOfWeek.Wednesday] = new DaySchedule(new Dictionary<int, ScheduleItem>
            {
                [0] = new ScheduleItem((ClassShortInfo) class1B, (TeacherShortInfo) t1, mathematics),
                [1] = new ScheduleItem((ClassShortInfo) class1B, (TeacherShortInfo) t2, history)
            }),
            [DayOfWeek.Thursday] = new DaySchedule(new Dictionary<int, ScheduleItem>
            {
                [0] = new ScheduleItem((ClassShortInfo) class1B, (TeacherShortInfo) t2, history),
                [1] = new ScheduleItem((ClassShortInfo) class1B, (TeacherShortInfo) t1, mathematics),
                [2] = new ScheduleItem((ClassShortInfo) class1B, (TeacherShortInfo) t1, mathematics)
            }),
            [DayOfWeek.Friday] = new DaySchedule(new Dictionary<int, ScheduleItem>
            {
                [0] = new ScheduleItem((ClassShortInfo) class1B, (TeacherShortInfo) t7, literacy),
                [1] = new ScheduleItem((ClassShortInfo) class1B, (TeacherShortInfo) t1, mathematics)
            })
        });

        await schedulerStorage.InsertScheduleAsync(educationPeriod, new SchoolClassesSchedule(new Dictionary<ClassShortInfo, ClassSchedule>
        {
            [(ClassShortInfo)class1A] = class1ASchedule,
            [(ClassShortInfo)class1B] = class1BSchedule
        }));

        return school;
    }
}