using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using ActiveStudy.Domain;
using ActiveStudy.Domain.Crm;
using ActiveStudy.Domain.Crm.Classes;
using ActiveStudy.Domain.Crm.Identity;
using ActiveStudy.Domain.Crm.Relatives;
using ActiveStudy.Domain.Crm.Schools;
using ActiveStudy.Domain.Crm.Students;
using ActiveStudy.Storage.Mongo.Identity;
using ActiveStudy.Web.Areas.Crm.Models.Students;
using ActiveStudy.Web.Models;
using ActiveStudy.Web.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ActiveStudy.Web.Areas.Crm.Controllers;

[Authorize]
[Area("Crm"), Route("school/{schoolId}/students")]
public class StudentsController : Controller
{
    private readonly ISchoolStorage schoolStorage;
    private readonly IStudentStorage studentStorage;
    private readonly IClassStorage classStorage;
    private readonly IRelativesStorage relativesStorage;
    private readonly IAuditStorage auditStorage;
    private readonly CurrentUserProvider currentUserProvider;
    private readonly IAccessResolver accessResolver;
    private readonly UserManager userManager;
    private readonly NotificationManager notificationManager;

    public StudentsController(ISchoolStorage schoolStorage,
        IStudentStorage studentStorage,
        IClassStorage classStorage,
        IRelativesStorage relativesStorage,
        IAuditStorage auditStorage,
        CurrentUserProvider currentUserProvider,
        IAccessResolver accessResolver,
        UserManager userManager,
        NotificationManager notificationManager)
    {
        this.schoolStorage = schoolStorage;
        this.studentStorage = studentStorage;
        this.classStorage = classStorage;
        this.relativesStorage = relativesStorage;
        this.auditStorage = auditStorage;
        this.currentUserProvider = currentUserProvider;
        this.accessResolver = accessResolver;
        this.userManager = userManager;
        this.notificationManager = notificationManager;
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> Details(
        [Required] string schoolId,
        [Required] string id)
    {
        if (!await accessResolver.HasReadAccessAsync(User, schoolId, Sections.Students))
        {
            return Forbid();
        }

        var student = await studentStorage.GetByIdAsync(id);
        var relatives = await relativesStorage.SearchAsync(student.Id);
        return View(new StudentDetailsPageModel(student.FullName, relatives));
    }

    [HttpGet("create")]
    public async Task<IActionResult> Create(string schoolId, string classId)
    {
        if (!await accessResolver.HasFullAccessAsync(User, schoolId, Sections.Students))
        {
            return Forbid();
        }

        return View(await Build(schoolId, classId));
    }

    [HttpPost("create")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create([Required]string schoolId, CreateStudentInputModel model)
    {
        if (!await accessResolver.HasFullAccessAsync(User, schoolId, Sections.Students))
        {
            return Forbid();
        }

        if (!ModelState.IsValid)
        {
            return View(await Build(schoolId, model?.ClassId, model));
        }

        // TODO: Validate create class access to school
        ClassShortInfo classInfo = null;
        if (!string.IsNullOrEmpty(model.ClassId))
        {
            var @class = await classStorage.GetByIdAsync(model.ClassId);
            classInfo = new ClassShortInfo(@class.Id, @class.Title);
        }

        var student = new Student(string.Empty, model.FirstName, model.LastName, model.Email, model.Phone, schoolId, null, classInfo == null ? Enumerable.Empty<ClassShortInfo>() : new [] { classInfo });
        var studentId = await studentStorage.InsertAsync(student);

        var school = await schoolStorage.GetByIdAsync(schoolId);
        await auditStorage.LogStudentCreateAsync(school.Id, school.Title,
            studentId, student.FullName,
            currentUserProvider.User.AsUser());
        foreach (var @class in student.Classes)
        {
            await auditStorage.LogStudentAddedToClassAsync(school.Id, school.Title,
                studentId, student.FullName,
                @class.Id, @class.Title,
                currentUserProvider.User.AsUser());
        }

        return RedirectToAction("Details", "Classes", new { schoolId, id = model.ClassId });
    }

    [HttpPost("{id}/invite")]
    public async Task<IActionResult> Invite([Required] string schoolId, [Required] string id)
    {
        if (!await accessResolver.HasFullAccessAsync(User, schoolId, Sections.Students))
        {
            return Forbid();
        }

        var student = await studentStorage.GetByIdAsync(id);
        var school = await schoolStorage.GetByIdAsync(schoolId);

        var user = new ActiveStudyUserEntity
        {
            UserName = student.Email,
            Email = student.Email,
            FirstName = student.FirstName,
            LastName = student.LastName
        };

        var existingUser = await userManager.FindByNameAsync(user.UserName);
        if (existingUser != null)
        {
            // var schoolIds = await userManager.GetSchoolClaimsAsync(existingUser);
            // if (schoolIds.Any(s => s.Value == schoolId))
            // {
            //     // TODO: Show error
            //     return RedirectToAction("List", new {schoolId});
            // }

            await userManager.AddToRoleAsync(existingUser, Role.Student, schoolId);
            await SendInvitationEmail(school, existingUser);
            await studentStorage.SetUserIdAsync(student.Id, existingUser.Id);
        }
        else
        {
            var result = await userManager.CreateAsync(user);
            if (result.Succeeded)
            {
                await userManager.AddToRoleAsync(user, Role.Student, schoolId);
                await SendInvitationEmail(school, user);
                await studentStorage.SetUserIdAsync(student.Id, user.Id);
            }
        }

        await auditStorage.LogStudentInvitedAsync(school.Id, school.Title,
            student.Id, student.FullName,
            currentUserProvider.User.AsUser());
            
        return RedirectToAction("Details", "School", new {id = schoolId});
    }
    
    private async Task<CreateStudentViewModel> Build(string schoolId, string classId, CreateStudentInputModel input = null)
    {
        var school = await schoolStorage.GetByIdAsync(schoolId);
            
        var @class = await classStorage.GetByIdAsync(classId);
        var classInfo = new ClassShortInfo(@class.Id, @class.Title);

        return new CreateStudentViewModel(school,
            classInfo,
            input?.FirstName,
            input?.LastName,
            input?.Email,
            input?.Phone,
            input?.ClassId);
    }

    private async Task SendInvitationEmail(School school, ActiveStudyUserEntity user)
    {
        var url = Url.Action(
            "CompleteInvitation",
            "Account",
            new
            {
                userId = user.Id,
                code = await userManager.GenerateEmailConfirmationTokenAsync(user)
            }, 
            Request.Scheme);

        await notificationManager.SendInvitationEmailAsync(new InvitationEmailTemplateInfo(
            user, school, url, currentUserProvider.User));
    }
}