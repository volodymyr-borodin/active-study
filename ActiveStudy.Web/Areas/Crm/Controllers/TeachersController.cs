using System;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using ActiveStudy.Domain;
using ActiveStudy.Domain.Crm.Classes;
using ActiveStudy.Domain.Crm.Identity;
using ActiveStudy.Domain.Crm.Scheduler;
using ActiveStudy.Domain.Crm.Schools;
using ActiveStudy.Domain.Crm.Teachers;
using ActiveStudy.Storage.Mongo.Identity;
using ActiveStudy.Web.Areas.Crm.Models.Teachers;
using ActiveStudy.Web.Models;
using ActiveStudy.Web.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace ActiveStudy.Web.Areas.Crm.Controllers;

[Authorize]
[Area("Crm"), Route("school/{schoolId}/teachers")]
public class TeachersController : Controller
{
    private readonly ITeacherStorage teacherStorage;
    private readonly ISchoolStorage schoolStorage;
    private readonly IClassStorage classStorage;
    private readonly IAuditStorage auditStorage;
    private readonly ISchedulerStorage scheduleStorage;
    private readonly CurrentUserProvider currentUserProvider;
    private readonly UserManager userManager;
    private readonly NotificationManager notificationManager;
    private readonly IAccessResolver accessResolver;

    public TeachersController(ITeacherStorage teacherStorage,
        ISchoolStorage schoolStorage,
        IClassStorage classStorage,
        ISchedulerStorage scheduleStorage,
        IAuditStorage auditStorage,
        CurrentUserProvider currentUserProvider,
        UserManager userManager,
        NotificationManager notificationManager,
        IAccessResolver accessResolver)
    {
        this.teacherStorage = teacherStorage;
        this.schoolStorage = schoolStorage;
        this.classStorage = classStorage;
        this.scheduleStorage = scheduleStorage;
        this.auditStorage = auditStorage;
        this.currentUserProvider = currentUserProvider;
        this.userManager = userManager;
        this.notificationManager = notificationManager;
        this.accessResolver = accessResolver;
    }
    
    [HttpGet]
    public async Task<IActionResult> List([Required]string schoolId)
    {
        var school = await schoolStorage.GetByIdAsync(schoolId);
        var teachers = await teacherStorage.FindAsync(schoolId);

        return View(new TeachersListPageModel(school, teachers));
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> Details([Required] string schoolId,
        [Required] string id,
        string scheduleDate = null)
    {
        var teacher = await teacherStorage.GetByIdAsync(id);
        var school = await schoolStorage.GetByIdAsync(teacher.SchoolId);

        var scheduleFrom = DateTime.Today.NearestMonday();
        if (!string.IsNullOrEmpty(scheduleDate))
        {
            if (DateTime.TryParse(scheduleDate, out var sFrom))
            {
                scheduleFrom = sFrom;
            }
        }
        var scheduleTo = scheduleFrom.AddDays(7);

        var schedule = await scheduleStorage.GetByTeacherAsync(id, scheduleFrom, scheduleTo);

        var model = new TeacherDetailsViewModel(teacher.Id,
            teacher.FullName,
            school,
            schedule);

        return View(model);
    }
    
    [HttpGet("create")]
    public async Task<IActionResult> Create([Required]string schoolId)
    {
        if (!await accessResolver.HasFullAccessAsync(User, schoolId, Sections.Teachers))
        {
            return Forbid();
        }

        return View(await Build(schoolId));
    }

    [HttpPost("create")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create([Required]string schoolId, CreateTeacherInputModel model)
    {
        if (!await accessResolver.HasFullAccessAsync(User, schoolId, Sections.Teachers))
        {
            return Forbid();
        }

        if (!ModelState.IsValid)
        {
            return View(await Build(schoolId));
        }

        var school = await schoolStorage.GetByIdAsync(schoolId);

        // TODO: Validate create class access to school
        var subjects = await schoolStorage.GetSubjectsAsync(schoolId, model.SubjectIds);

        var teacher = new Teacher(string.Empty, model.FirstName, model.LastName, model.MiddleName, model.Email, subjects, schoolId, null);
        var teacherId = await teacherStorage.InsertAsync(teacher);

        await auditStorage.LogTeacherCreatedAsync(school.Id, school.Title,
            teacherId, teacher.FullName,
            currentUserProvider.User.AsUser());
    
        return RedirectToAction("List", "Teachers", new { schoolId });
    }

    [HttpPost("{id}/delete")]
    public async Task<IActionResult> Delete([Required] string schoolId, [Required] string id)
    {
        if (!await accessResolver.HasFullAccessAsync(User, schoolId, Sections.Teachers))
        {
            return Forbid();
        }

        var teacher = await teacherStorage.GetByIdAsync(id);

        // TODO: Add validation. Teacher can be assigned to class
        await teacherStorage.DeleteAsync(teacher);
            
        var school = await schoolStorage.GetByIdAsync(schoolId);
        await auditStorage.LogTeacherRemovedAsync(school.Id, school.Title,
            teacher.Id, teacher.FullName,
            currentUserProvider.User.AsUser());

        return RedirectToAction("List", "Teachers", new {schoolId});
    }

    [HttpPost("{id}/invite")]
    public async Task<IActionResult> Invite([Required] string schoolId, [Required] string id)
    {
        if (!await accessResolver.HasFullAccessAsync(User, schoolId, Sections.Teachers))
        {
            return Forbid();
        }

        var teacher = await teacherStorage.GetByIdAsync(id);
        var school = await schoolStorage.GetByIdAsync(schoolId);

        var user = new ActiveStudyUserEntity
        {
            UserName = teacher.Email,
            Email = teacher.Email,
            FirstName = teacher.FirstName,
            LastName = teacher.LastName
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

            await userManager.AddToRoleAsync(existingUser, Role.Teacher, schoolId);
            await SendInvitationEmail(school, existingUser);
            await teacherStorage.SetUserIdAsync(teacher.Id, existingUser.Id);
            await classStorage.SetTeacherUserIdAsync(teacher.Id, existingUser.Id);
        }
        else
        {
            var result = await userManager.CreateAsync(user);
            if (result.Succeeded)
            {
                await userManager.AddToRoleAsync(user, Role.Teacher, schoolId);
                await SendInvitationEmail(school, user);
                await teacherStorage.SetUserIdAsync(teacher.Id, user.Id);
                await classStorage.SetTeacherUserIdAsync(teacher.Id, user.Id);
            }
        }

        await auditStorage.LogTeacherInvitedAsync(school.Id, school.Title,
            teacher.Id, teacher.FullName,
            currentUserProvider.User.AsUser());
            
        return RedirectToAction("List", new {schoolId});
    }

    private async Task<CreateTeacherViewModel> Build(string schoolId, CreateTeacherInputModel input = null)
    {
        var school = await schoolStorage.GetByIdAsync(schoolId);
        var subjects = await schoolStorage.GetSubjectsAsync(school.Id);

        return new CreateTeacherViewModel(
            school,
            new SelectList(subjects, "Id", "Title"),
            input?.FirstName,
            input?.LastName,
            input?.MiddleName,
            input?.Email,
            input?.SubjectIds);
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