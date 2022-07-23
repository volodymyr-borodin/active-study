using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using ActiveStudy.Domain;
using ActiveStudy.Domain.Crm.Identity;
using ActiveStudy.Domain.Crm.Schools;
using ActiveStudy.Domain.Crm.Teachers;
using ActiveStudy.Storage.Mongo.Identity;
using ActiveStudy.Web.Areas.Crm.Models.Schools;
using ActiveStudy.Web.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using static ActiveStudy.Web.Consts;

namespace ActiveStudy.Web.Areas.Crm.Controllers;

[Authorize]
[Area(Area.Crm), Route("school")]
public class SchoolController : Controller
{
    private const string CountryCode = "UA";
    private readonly ICountryStorage countryStorage;
    private readonly ISchoolStorage schoolStorage;
    private readonly CurrentUserProvider currentUserProvider;
    private readonly UserManager userManager;
    private readonly RoleManager roleManager;

    private readonly ITeacherStorage teacherStorage;
    private readonly IAuditStorage auditStorage;
    private readonly DemoSchool demoSchool;

    public SchoolController(ISchoolStorage schoolStorage,
        ICountryStorage countryStorage,
        CurrentUserProvider currentUserProvider,
        UserManager userManager,
        RoleManager roleManager,
        ITeacherStorage teacherStorage,
        IAuditStorage auditStorage,
        DemoSchool demoSchool)
    {
        this.schoolStorage = schoolStorage;
        this.countryStorage = countryStorage;
        this.currentUserProvider = currentUserProvider;
        this.userManager = userManager;
        this.roleManager = roleManager;
        this.teacherStorage = teacherStorage;
        this.auditStorage = auditStorage;
        this.demoSchool = demoSchool;
    }

    [HttpGet("create")]
    public async Task<IActionResult> Create()
    {
        return View(await BuildCreateModel());
    }

    [HttpPost]
    public async Task<IActionResult> CreateDemo()
    {
        var school = await demoSchool.InitAsync(currentUserProvider.User.AsUser());
        await roleManager.AddDefaultAsync(school.Id);
        await userManager.AddToRoleAsync(currentUserProvider.User, Role.Principal, school.Id);

        return RedirectToAction("Details", new {id = school.Id});
    }

    [HttpPost("create")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(CreateSchoolInputModel model)
    {
        if (!ModelState.IsValid)
        {
            return View(await BuildCreateModel());
        }

        var country = await countryStorage.GetByCodeAsync(CountryCode);
        var user = currentUserProvider.User.AsUser();
        var school = new School(null, model.Title, model.Description ?? string.Empty, country, user);

        var schoolId = await schoolStorage.CreateAsync(school);
        var subjects = model.Subjects.Select(s => new Subject(null, s)).ToList();
        await schoolStorage.InsertSubjectsAsync(schoolId, subjects);
        await roleManager.AddDefaultAsync(schoolId);
        await auditStorage.LogSchoolCreateAsync(schoolId, school.Title, user);

        await userManager.AddToRoleAsync(currentUserProvider.User, Role.Principal, schoolId);

        var teacher = new Teacher(string.Empty, currentUserProvider.User.FirstName,
            currentUserProvider.User.LastName, string.Empty, currentUserProvider.User.Email, Enumerable.Empty<Subject>(),
            schoolId, currentUserProvider.User.Id);
        var teacherId = await teacherStorage.InsertAsync(teacher);

        await auditStorage.LogTeacherCreatedAsync(schoolId, school.Title,
            teacherId, teacher.FullName, user);
            
        return RedirectToAction("Index", "Home");
    }

    [HttpGet("")]
    public async Task<IActionResult> List()
    {
        var schoolIds = await currentUserProvider.GetAssignedSchoolAsync();
        var schools = await schoolStorage.SearchByIdsAsync(schoolIds);

        return View(new SchoolListPageModel(schools));
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> Details([Required] string id, [FromQuery]ScheduleDisplayMode dm = ScheduleDisplayMode.ByClasses)
    {
        return View(await BuildIndexModel(id, dm));
    }

    // [HttpGet("{id}/audit")]
    // public async Task<IActionResult> Audit([Required] string id)
    // {
    //     return View(await BuildIndexModel(id));
    // }

    private async Task<SchoolHomePageModel> BuildIndexModel(string schoolId, ScheduleDisplayMode displayMode)
    {
        var school = await schoolStorage.GetByIdAsync(schoolId);

        return new SchoolHomePageModel(school, displayMode);
    }

    private async Task<CreateSchoolModel> BuildCreateModel()
    {
        var countries = await countryStorage.SearchAsync();

        return new CreateSchoolModel(new SelectList(countries, "Code", "Name"),
            string.Empty,
            string.Empty,
            string.Empty,
            Enumerable.Range(0, 6).Select(_ => string.Empty).ToArray());
    }
}