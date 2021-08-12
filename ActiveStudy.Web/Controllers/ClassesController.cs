using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using ActiveStudy.Domain.Crm;
using ActiveStudy.Domain.Crm.Classes;
using ActiveStudy.Domain.Crm.Schools;
using ActiveStudy.Domain.Crm.Students;
using ActiveStudy.Domain.Crm.Teachers;
using ActiveStudy.Web.Models.Classes;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using StudentListModel = ActiveStudy.Web.Models.Classes.StudentListModel;

namespace ActiveStudy.Web.Areas.Schools.Controllers
{
    [Route("school/{schoolId}/classes"), Authorize]
    public class ClassesController : Controller
    {
        private readonly IClassStorage classStorage;
        private readonly ISchoolStorage schoolStorage;
        private readonly IStudentStorage studentStorage;
        private readonly ITeacherStorage teacherStorage;

        public ClassesController(IClassStorage classStorage,
            ISchoolStorage schoolStorage,
            IStudentStorage studentStorage,
            ITeacherStorage teacherStorage)
        {
            this.classStorage = classStorage;
            this.studentStorage = studentStorage;
            this.teacherStorage = teacherStorage;
            this.schoolStorage = schoolStorage;
        }

        [HttpGet]
        public async Task<IActionResult> List([Required]string schoolId)
        {
            var school = await schoolStorage.GetByIdAsync(schoolId);
            var classes = await classStorage.FindAsync(schoolId);

            return View(new ClassesListPageModel(school, classes.OrderBy(c => c.Grade).ThenBy(c => c.Label)));
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Details([Required] string schoolId, [Required] string id)
        {
            var @class = await classStorage.GetByIdAsync(id);
            var students = await studentStorage.FindAsync(StudentFilter.ByClass(id));
            var school = await schoolStorage.GetByIdAsync(@class.SchoolId);
            
            var model = new ClassViewModel(@class.Id, school, @class.Title, @class.Teacher, students.Select(s => new StudentListModel(s.Id, s.FullName)));
            
            return View(model);
        }

        [HttpGet("create")]
        public async Task<IActionResult> Create(string schoolId)
        {
            return View(new CreateClassViewModel(
                await teacherStorage.FindAsync(schoolId)));
        }

        [HttpPost("create")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Required]string schoolId, CreateClassInputModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(new CreateClassViewModel(await teacherStorage.FindAsync(schoolId))
                {
                    Label = model.Label,
                    Grade = model.Grade
                });
            }

            var teacher = string.IsNullOrEmpty(model.TeacherId)
                ? null
                : await teacherStorage.GetByIdAsync(model.TeacherId);

            // TODO: Validate create class access to school
            var @class = new Class(string.Empty, model.Grade, model.Label, (TeacherShortInfo) teacher, schoolId);
            var classId = await classStorage.InsertAsync(@class);

            return RedirectToAction("Details", new { schoolId, id = classId });
        }
    }
}
