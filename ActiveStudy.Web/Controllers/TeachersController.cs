using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using ActiveStudy.Domain;
using ActiveStudy.Domain.Crm.Classes;
using ActiveStudy.Domain.Crm.Scheduler;
using ActiveStudy.Domain.Crm.Schools;
using ActiveStudy.Domain.Crm.Teachers;
using ActiveStudy.Storage.Mongo.Identity;
using ActiveStudy.Web.Models;
using ActiveStudy.Web.Models.Classes;
using ActiveStudy.Web.Models.Teachers;
using ActiveStudy.Web.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace ActiveStudy.Web.Areas.Schools.Controllers
{
    [Route("school/{schoolId}/teachers"), Authorize]
    public class TeachersController : Controller
    {
        private readonly ITeacherStorage teacherStorage;
        private readonly ISchoolStorage schoolStorage;
        private readonly ISubjectStorage subjectStorage;
        private readonly IClassStorage classStorage;
        private readonly IAuditStorage auditStorage;
        private readonly ISchedulerStorage scheduleStorage;
        private readonly CurrentUserProvider currentUserProvider;
        private readonly UserManager<ActiveStudyUserEntity> userManager;
        private readonly NotificationManager notificationManager;

        public TeachersController(ITeacherStorage teacherStorage,
            ISchoolStorage schoolStorage,
            ISubjectStorage subjectStorage,
            IClassStorage classStorage,
            ISchedulerStorage scheduleStorage,
            IAuditStorage auditStorage,
            CurrentUserProvider currentUserProvider,
            UserManager<ActiveStudyUserEntity> userManager,
            NotificationManager notificationManager)
        {
            this.teacherStorage = teacherStorage;
            this.schoolStorage = schoolStorage;
            this.subjectStorage = subjectStorage;
            this.classStorage = classStorage;
            this.scheduleStorage = scheduleStorage;
            this.auditStorage = auditStorage;
            this.currentUserProvider = currentUserProvider;
            this.userManager = userManager;
            this.notificationManager = notificationManager;
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

            var schedule = await scheduleStorage.GetByClassAsync(id, scheduleFrom, scheduleTo);

            var model = new TeacherDetailsViewModel(teacher.Id,
                teacher.FullName,
                school,
                schedule);

            return View(model);
        }
    
        [HttpGet("create")]
        public async Task<IActionResult> Create([Required]string schoolId)
        {
            return View(await Build(schoolId));
        }

        [HttpPost("create")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Required]string schoolId, CreateTeacherInputModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(await Build(schoolId));
            }

            var school = await schoolStorage.GetByIdAsync(schoolId);

            // TODO: Validate create class access to school
            var subjects = await subjectStorage.SearchAsync(model.SubjectIds);
            
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
                var schoolIds = await userManager.GetSchoolClaimsAsync(existingUser);
                if (schoolIds.Any(s => s.Value == schoolId))
                {
                    // TODO: Show error
                    return RedirectToAction("List", new {schoolId});
                }

                await userManager.AddSchoolClaimAsync(existingUser, schoolId);
                await SendInvitationEmail(school, existingUser);
                await teacherStorage.SetUserIdAsync(teacher.Id, existingUser.Id);
                await classStorage.SetTeacherUserIdAsync(teacher.Id, existingUser.Id);
            }
            else
            {
                var result = await userManager.CreateAsync(user);
                if (result.Succeeded)
                {
                    await userManager.AddSchoolClaimAsync(user, schoolId);
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
            var subjects = await subjectStorage.SearchAsync(school.Country.Code);

            return new CreateTeacherViewModel(school, subjects)
            {
                FirstName = input?.FirstName,
                LastName = input?.LastName,
                Email = input?.Email,
                SubjectIds = input?.SubjectIds
            };
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
}
