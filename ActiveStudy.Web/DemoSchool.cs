using System.Linq;
using System.Threading.Tasks;
using ActiveStudy.Domain;
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

    private static Subject Geography = new Subject("en-geography", "Geography");
    private static Subject History = new Subject("en-history", "History");
    private static Subject Literacy = new Subject("en-literacy", "Literacy");
    private static Subject Music = new Subject("en-music", "Music");
    private static Subject Mathematics = new Subject("en-mathematics", "Mathematics");

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
        var school = new School(ObjectId.GenerateNewId().ToString(), "Demo", string.Empty, new Country("Україна", "UA"), owner);
        await schoolStorage.CreateAsync(school);

        // init teachers
        await teacherStorage.InsertAsync(new Teacher(ObjectId.GenerateNewId().ToString(),
            "Анастасія", "Боднаренко", "Боднаренко", "a.bodnarenko@yopmail.com",
            new[] {Mathematics},
            school.Id, null));

        await teacherStorage.InsertAsync(new Teacher(ObjectId.GenerateNewId().ToString(),
            "Ярослав", "Романченко", "Іванович", "y.romanchenko@yopmail.com",
            new[] {History},
            school.Id, null));

        await teacherStorage.InsertAsync(new Teacher(ObjectId.GenerateNewId().ToString(),
            "Ольга", "Пономарчук", "Євгеніївна", "o.ponomarchuk@yopmail.com",
            new[] {Literacy},
            school.Id, null));

        await teacherStorage.InsertAsync(new Teacher(ObjectId.GenerateNewId().ToString(),
            "Валерія", "Броваренко", "Олександрівна", "v.brovarenko@yopmail.com",
            new[] {Music},
            school.Id, null));

        await teacherStorage.InsertAsync(new Teacher(ObjectId.GenerateNewId().ToString(),
            "Данил", "Іванченко", "Федорович", "d.ivanchenko@yopmail.com",
            new[] {Geography},
            school.Id, null));

        // init classes
        foreach (var grade in Enumerable.Range(1, 4))
        {
            foreach (var label in new [] {"A", "B"})
            {
                await classStorage.InsertAsync(new Class(ObjectId.GenerateNewId().ToString(), grade, label, null, school.Id));
            }
        }

        return school;
    }
}