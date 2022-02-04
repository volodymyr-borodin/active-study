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
        await teacherStorage.InsertAsync(new Teacher(ObjectId.GenerateNewId().ToString(),
            "Анастасія", "Боднаренко", "Боднаренко", "a.bodnarenko@yopmail.com",
            new[] {mathematics},
            school.Id, null));

        await teacherStorage.InsertAsync(new Teacher(ObjectId.GenerateNewId().ToString(),
            "Ярослав", "Романченко", "Іванович", "y.romanchenko@yopmail.com",
            new[] {history},
            school.Id, null));

        await teacherStorage.InsertAsync(new Teacher(ObjectId.GenerateNewId().ToString(),
            "Ольга", "Пономарчук", "Євгеніївна", "o.ponomarchuk@yopmail.com",
            new[] {literacy},
            school.Id, null));

        await teacherStorage.InsertAsync(new Teacher(ObjectId.GenerateNewId().ToString(),
            "Валерія", "Броваренко", "Олександрівна", "v.brovarenko@yopmail.com",
            new[] {music},
            school.Id, null));

        await teacherStorage.InsertAsync(new Teacher(ObjectId.GenerateNewId().ToString(),
            "Данил", "Іванченко", "Федорович", "d.ivanchenko@yopmail.com",
            new[] {geography},
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