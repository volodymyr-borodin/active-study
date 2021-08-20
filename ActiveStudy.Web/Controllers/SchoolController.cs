using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using ActiveStudy.Domain;
using ActiveStudy.Domain.Crm.Schools;
using ActiveStudy.Domain.Crm.Teachers;
using ActiveStudy.Storage.Mongo.Identity;
using ActiveStudy.Web.Models;
using ActiveStudy.Web.Models.Schools;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace ActiveStudy.Web.Controllers
{
    [Authorize, Route("school")]
    public class SchoolController : Controller
    {
        private const string CountryCode = "UA";
        private readonly ICountryStorage countryStorage;
        private readonly ISchoolStorage schoolStorage;
        private readonly CurrentUserProvider currentUserProvider;
        private readonly UserManager<ActiveStudyUserEntity> userManager;

        private readonly ITeacherStorage teacherStorage;
        private readonly IAuditStorage auditStorage;

        public SchoolController(ISchoolStorage schoolStorage,
            ICountryStorage countryStorage,
            CurrentUserProvider currentUserProvider,
            UserManager<ActiveStudyUserEntity> userManager,
            ITeacherStorage teacherStorage,
            IAuditStorage auditStorage)
        {
            this.schoolStorage = schoolStorage;
            this.countryStorage = countryStorage;
            this.currentUserProvider = currentUserProvider;
            this.userManager = userManager;
            this.teacherStorage = teacherStorage;
            this.auditStorage = auditStorage;
        }

        [HttpGet("create")]
        public async Task<IActionResult> Create()
        {
            return View(await BuildCreateModel());
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
            var school = new School(null, model.Title, country, user);

            var schoolId = await schoolStorage.CreateAsync(school);
            await LogSchoolCreate(schoolId, school.Title, user);
            
            await userManager.AddSchoolClaimAsync(await userManager.GetUserAsync(User), schoolId);

            var teacher = new Teacher(string.Empty, currentUserProvider.User.FirstName,
                currentUserProvider.User.LastName, string.Empty, currentUserProvider.User.Email, Enumerable.Empty<Subject>(),
                schoolId, currentUserProvider.User.Id);
            var teacherId = await teacherStorage.InsertAsync(teacher);

            await LogTeacherAdded(schoolId, school.Title, teacherId, teacher.FullName, user);
            
            return RedirectToAction("Index", "Home");
        }

        [HttpGet("list")]
        public async Task<IActionResult> List()
        {
            var schoolIds = await currentUserProvider.GetAssignedSchoolAsync();
            var schools = await schoolStorage.SearchByIdsAsync(schoolIds);

            return View(new SchoolListPageModel(schools));
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Details([Required] string id)
        {
            return View(await BuildIndexModel(id));
        }

        [HttpGet("{id}/audit")]
        public async Task<IActionResult> Audit([Required] string id)
        {
            return View(await BuildIndexModel(id));
        }

        private async Task<SchoolHomePageModel> BuildIndexModel(string schoolId)
        {
            var school = await schoolStorage.GetByIdAsync(schoolId);

            return new SchoolHomePageModel(school);
        }

        private async Task<CreateSchoolModel> BuildCreateModel()
        {
            var countries = await countryStorage.SearchAsync();

            return new CreateSchoolModel(countries);
        }

        private async Task LogSchoolCreate(string schoolId, string schoolTitle, User user)
        {
            await auditStorage.LogAsync(new AuditItem($"School {schoolTitle} has created", user, new List<AuditEntity>
            {
                new AuditEntity(schoolId, EntityType.School)
            }));
        }

        private async Task LogTeacherAdded(string schoolId, string schoolTitle,
            string teacherId, string teacherName, User user)
        {
            await auditStorage.LogAsync(new AuditItem($"Teacher {teacherName} has added to {schoolTitle} school", user, new List<AuditEntity>
            {
                new AuditEntity(schoolId, EntityType.School),
                new AuditEntity(teacherId, EntityType.Teacher)
            }));
        }
    }
}