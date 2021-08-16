using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using ActiveStudy.Domain.Crm;
using ActiveStudy.Domain.Crm.Classes;
using ActiveStudy.Domain.Crm.Scheduler;
using ActiveStudy.Domain.Crm.Schools;
using ActiveStudy.Domain.Crm.Students;
using ActiveStudy.Domain.Crm.Teachers;
using ActiveStudy.Web.Models.Classes;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ActiveStudy.Web.Areas.Schools.Controllers
{
    [Route("school/{schoolId}/classes"), Authorize]
    public class ClassesController : Controller
    {
        private readonly IClassStorage classStorage;
        private readonly ISchoolStorage schoolStorage;
        private readonly IStudentStorage studentStorage;
        private readonly ITeacherStorage teacherStorage;
        private readonly ISchedulerStorage scheduleStorage;

        public ClassesController(IClassStorage classStorage,
            ISchoolStorage schoolStorage,
            IStudentStorage studentStorage,
            ITeacherStorage teacherStorage,
            ISchedulerStorage scheduleStorage)
        {
            this.classStorage = classStorage;
            this.studentStorage = studentStorage;
            this.teacherStorage = teacherStorage;
            this.scheduleStorage = scheduleStorage;
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
            var schedule = await scheduleStorage.GetByClassAsync(id, DateTime.Today, DateTime.Today.AddDays(7));

            var model = new ClassViewModel(@class.Id, @class.Title, school, @class.Teacher, students, schedule);

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
