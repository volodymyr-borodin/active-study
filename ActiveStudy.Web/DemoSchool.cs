using System;
using System.Threading.Tasks;
using ActiveStudy.Domain;
using ActiveStudy.Domain.Crm;
using ActiveStudy.Domain.Crm.Classes;
using ActiveStudy.Domain.Crm.Schools;
using ActiveStudy.Domain.Crm.Teachers;
using MongoDB.Bson;

namespace ActiveStudy.Web;

public class DemoSchool
{
    private readonly ISchoolStorage schoolStorage;
    private readonly ITeacherStorage teacherStorage;
    private readonly IClassStorage classStorage;

    public DemoSchool(
        ISchoolStorage schoolStorage,
        ITeacherStorage teacherStorage,
        IClassStorage classStorage)
    {
        this.schoolStorage = schoolStorage;
        this.teacherStorage = teacherStorage;
        this.classStorage = classStorage;
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
        
        var from = new DateTime(DateTime.Today.Year, 1, 1);
        var to = new DateTime(DateTime.Today.Year, 12, 31);

        // var (class1ASchedule, _) = ClassScheduleTemplate.New(DateOnly.FromDateTime(from),
        //     DateOnly.FromDateTime(to), new List<SchedulePeriod>
        //     {
        //         new SchedulePeriod(new TimeOnly(8, 30), new TimeOnly(9, 15),
        //             new Dictionary<DayOfWeek, ScheduleTemplateLesson>
        //             {
        //                 [DayOfWeek.Monday] = new ScheduleTemplateLesson((ClassShortInfo)class1A, (TeacherShortInfo)t1, mathematics),
        //                 [DayOfWeek.Tuesday] = new ScheduleTemplateLesson((ClassShortInfo)class1A, (TeacherShortInfo)t4, music),
        //             }),
        //         new SchedulePeriod(new TimeOnly(9, 30), new TimeOnly(10, 15),
        //             new Dictionary<DayOfWeek, ScheduleTemplateLesson>()
        //             {
        //                 [DayOfWeek.Monday] = new ScheduleTemplateLesson((ClassShortInfo)class1A, (TeacherShortInfo)t1, mathematics),
        //                 [DayOfWeek.Tuesday] = new ScheduleTemplateLesson((ClassShortInfo)class1A, (TeacherShortInfo)t3, literacy),
        //             }),
        //         new SchedulePeriod(new TimeOnly(10, 30), new TimeOnly(11, 15),
        //             new Dictionary<DayOfWeek, ScheduleTemplateLesson>()),
        //         new SchedulePeriod(new TimeOnly(11, 30), new TimeOnly(12, 15),
        //             new Dictionary<DayOfWeek, ScheduleTemplateLesson>()),
        //         new SchedulePeriod(new TimeOnly(12, 30), new TimeOnly(13, 15),
        //             new Dictionary<DayOfWeek, ScheduleTemplateLesson>())
        //     });
        // await classStorage.SaveScheduleTemplateAsync(class1A.Id, class1ASchedule);
        //
        // var (class1BSchedule, _) = ClassScheduleTemplate.New(DateOnly.FromDateTime(from),
        //     DateOnly.FromDateTime(to), new List<SchedulePeriod>
        //     {
        //         new SchedulePeriod(new TimeOnly(8, 30), new TimeOnly(9, 15),
        //             new Dictionary<DayOfWeek, ScheduleTemplateLesson>
        //             {
        //                 [DayOfWeek.Monday] = new ScheduleTemplateLesson((ClassShortInfo)class1B, (TeacherShortInfo)t6, mathematics),
        //                 [DayOfWeek.Tuesday] = new ScheduleTemplateLesson((ClassShortInfo)class1B, (TeacherShortInfo)t6, mathematics),
        //             }),
        //         new SchedulePeriod(new TimeOnly(9, 30), new TimeOnly(10, 15),
        //             new Dictionary<DayOfWeek, ScheduleTemplateLesson>()
        //             {
        //                 [DayOfWeek.Monday] = new ScheduleTemplateLesson((ClassShortInfo)class1B, (TeacherShortInfo)t3, literacy),
        //                 [DayOfWeek.Tuesday] = new ScheduleTemplateLesson((ClassShortInfo)class1B, (TeacherShortInfo)t4, music),
        //             }),
        //         new SchedulePeriod(new TimeOnly(10, 30), new TimeOnly(11, 15),
        //             new Dictionary<DayOfWeek, ScheduleTemplateLesson>()),
        //         new SchedulePeriod(new TimeOnly(11, 30), new TimeOnly(12, 15),
        //             new Dictionary<DayOfWeek, ScheduleTemplateLesson>()),
        //         new SchedulePeriod(new TimeOnly(12, 30), new TimeOnly(13, 15),
        //             new Dictionary<DayOfWeek, ScheduleTemplateLesson>())
        //     });
        // await classStorage.SaveScheduleTemplateAsync(class1B.Id, class1BSchedule);

        return school;
    }
}