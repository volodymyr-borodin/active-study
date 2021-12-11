using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using ActiveStudy.Domain;
using ActiveStudy.Domain.Crm;
using ActiveStudy.Domain.Crm.Classes;
using ActiveStudy.Domain.Crm.Relatives;
using ActiveStudy.Domain.Crm.Schools;
using ActiveStudy.Domain.Crm.Students;
using ActiveStudy.Web.Models;
using ActiveStudy.Web.Models.Students;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ActiveStudy.Web.Controllers
{
    [Route("school/{schoolId}/students"), Authorize]
    public class StudentsController : Controller
    {
        private readonly ISchoolStorage schoolStorage;
        private readonly IStudentStorage studentStorage;
        private readonly IClassStorage classStorage;
        private readonly IRelativesStorage relativesStorage;
        private readonly IAuditStorage auditStorage;
        private readonly CurrentUserProvider currentUserProvider;

        public StudentsController(ISchoolStorage schoolStorage,
            IStudentStorage studentStorage,
            IClassStorage classStorage,
            IRelativesStorage relativesStorage,
            IAuditStorage auditStorage,
            CurrentUserProvider currentUserProvider)
        {
            this.schoolStorage = schoolStorage;
            this.studentStorage = studentStorage;
            this.classStorage = classStorage;
            this.relativesStorage = relativesStorage;
            this.auditStorage = auditStorage;
            this.currentUserProvider = currentUserProvider;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Details([Required] string id)
        {
            var student = await studentStorage.GetByIdAsync(id);
            var relatives = await relativesStorage.SearchAsync(student.Id);
            return View(new StudentDetailsPageModel(student.FullName, relatives));
        }

        [HttpGet("create")]
        public async Task<IActionResult> Create(string schoolId, string classId)
        {
            return View(await Build(schoolId, classId));
        }

        [HttpPost("create")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Required]string schoolId, CreateStudentInputModel model)
        {
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

            var student = new Student(string.Empty, model.FirstName, model.LastName, model.Email, model.Phone, schoolId, classInfo == null ? Enumerable.Empty<ClassShortInfo>() : new [] { classInfo });
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
    
        private async Task<CreateStudentViewModel> Build(string schoolId, string classId, CreateStudentInputModel input = null)
        {
            var school = await schoolStorage.GetByIdAsync(schoolId);
            
            var @class = await classStorage.GetByIdAsync(classId);
            var classInfo = new ClassShortInfo(@class.Id, @class.Title);

            return new CreateStudentViewModel(school, classInfo)
            {
                FirstName = input?.FirstName,
                LastName = input?.LastName,
                Email = input?.Email
            };
        }
    }
}