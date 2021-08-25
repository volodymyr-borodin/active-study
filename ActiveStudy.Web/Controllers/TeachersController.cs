using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using ActiveStudy.Domain;
using ActiveStudy.Domain.Crm.Schools;
using ActiveStudy.Domain.Crm.Teachers;
using ActiveStudy.Storage.Mongo.Identity;
using ActiveStudy.Web.Models;
using ActiveStudy.Web.Models.Teachers;
using ActiveStudy.Web.Services.Email;
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
        private readonly IAuditStorage auditStorage;
        private readonly CurrentUserProvider currentUserProvider;
        private readonly UserManager<ActiveStudyUserEntity> userManager;
        private readonly IEmailService emailService;

        public TeachersController(ITeacherStorage teacherStorage,
            ISchoolStorage schoolStorage,
            ISubjectStorage subjectStorage,
            IAuditStorage auditStorage,
            CurrentUserProvider currentUserProvider, UserManager<ActiveStudyUserEntity> userManager, IEmailService emailService)
        {
            this.teacherStorage = teacherStorage;
            this.schoolStorage = schoolStorage;
            this.subjectStorage = subjectStorage;
            this.auditStorage = auditStorage;
            this.currentUserProvider = currentUserProvider;
            this.userManager = userManager;
            this.emailService = emailService;
        }
    
        [HttpGet]
        public async Task<IActionResult> List([Required]string schoolId)
        {
            var school = await schoolStorage.GetByIdAsync(schoolId);
            var teachers = await teacherStorage.FindAsync(schoolId);

            return View(new TeachersListPageModel(school, teachers));
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
            
            var teacher = new Teacher(string.Empty, model.FirstName, model.LastName, model.MiddleName, model.Email, subjects, schoolId, string.Empty);
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
                await SendInvitationEmail(existingUser);
            }
            else
            {
                var result = await userManager.CreateAsync(user);
                if (result.Succeeded)
                {
                    await userManager.AddSchoolClaimAsync(user, schoolId);
                    await SendInvitationEmail(user);
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

        private async Task SendInvitationEmail(ActiveStudyUserEntity user)
        {
            var code = await userManager.GenerateEmailConfirmationTokenAsync(user);
            var url = Url.Action(
                "CompleteInvitation", "Account",
                new { userId = user.Id, code }, 
                Request.Scheme);

            var subject = "Confirm your email";
            var body = $"You was invited to school. Please accept invitation clicking this link: <a href=\"{url}\">link</a>";

            await emailService.SendEmailAsync(new EmailRecipient($"{user.FirstName} {user.LastName}", user.Email), subject, body);
        }
    }
}
