using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using ActiveStudy.Domain;
using ActiveStudy.Domain.Crm;
using ActiveStudy.Domain.Crm.Classes;
using ActiveStudy.Domain.Crm.Relatives;
using ActiveStudy.Domain.Crm.Scheduler;
using ActiveStudy.Domain.Crm.Schools;
using ActiveStudy.Domain.Crm.Students;
using ActiveStudy.Domain.Crm.Teachers;
using ActiveStudy.Web.Models;
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
        private readonly IRelativesStorage relativesStorage;
        private readonly ITeacherStorage teacherStorage;
        private readonly ISchedulerStorage scheduleStorage;
        private readonly IAuditStorage auditStorage;
        private readonly CurrentUserProvider currentUserProvider;

        public ClassesController(IClassStorage classStorage,
            ISchoolStorage schoolStorage,
            IStudentStorage studentStorage,
            IRelativesStorage relativesStorage,
            ITeacherStorage teacherStorage,
            ISchedulerStorage scheduleStorage,
            IAuditStorage auditStorage,
            CurrentUserProvider currentUserProvider)
        {
            this.classStorage = classStorage;
            this.studentStorage = studentStorage;
            this.relativesStorage = relativesStorage;
            this.teacherStorage = teacherStorage;
            this.scheduleStorage = scheduleStorage;
            this.auditStorage = auditStorage;
            this.currentUserProvider = currentUserProvider;
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
        public async Task<IActionResult> Details([Required] string schoolId,
            [Required] string id,
            string scheduleDate = null)
        {
            var @class = await classStorage.GetByIdAsync(id);
            var students = await studentStorage.FindAsync(StudentFilter.ByClass(id));
            var school = await schoolStorage.GetByIdAsync(@class.SchoolId);

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

            var relatives = await relativesStorage.SearchAsync(students.Select(s => s.Id));

            var model = new ClassViewModel(@class.Id,
                @class.Title,
                school,
                @class.Teacher,
                students.Select(s => new StudentViewModel(
                        s.Id,
                        s.FullName,
                        relatives[s.Id]
                            .Select(r => new RelativeViewModel(r.Id, r.FullName, r.Phone))
                            .ToList()))
                    .ToList(),
                schedule);

            return View(model);
        }

        [HttpGet("{id}/students")]
        public async Task<IActionResult> Students([Required] string schoolId,
            [Required] string id,
            string scheduleDate = null)
        {
            var @class = await classStorage.GetByIdAsync(id);
            var students = await studentStorage.FindAsync(StudentFilter.ByClass(id));
            var school = await schoolStorage.GetByIdAsync(@class.SchoolId);

            var scheduleFrom = DateTime.Today.NearestMonday();
            if (!string.IsNullOrEmpty(scheduleDate))
            {
                DateTime.TryParse(scheduleDate, out scheduleFrom);
            }
            var scheduleTo = scheduleFrom.AddDays(7);

            var schedule = await scheduleStorage.GetByClassAsync(id, scheduleFrom, scheduleTo);

            var relatives = await relativesStorage.SearchAsync(students.Select(s => s.Id));

            var model = new ClassViewModel(@class.Id,
                @class.Title,
                school,
                @class.Teacher,
                students.Select(s => new StudentViewModel(
                    s.Id,
                    s.FullName,
                    relatives[s.Id]
                        .Select(r => new RelativeViewModel(r.Id, r.FullName, r.Phone))
                        .ToList()))
                    .ToList(),
                schedule);

            return View(model);
        }

        [HttpGet("create")]
        public async Task<IActionResult> Create(string schoolId)
        {
            return View(new CreateClassViewModel(
                await schoolStorage.GetByIdAsync(schoolId),
                await teacherStorage.FindAsync(schoolId)));
        }

        [HttpPost("create")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Required]string schoolId, CreateClassInputModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(new CreateClassViewModel(
                    await schoolStorage.GetByIdAsync(schoolId),
                    await teacherStorage.FindAsync(schoolId))
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

            var school = await schoolStorage.GetByIdAsync(schoolId);
            await auditStorage.LogClassCreatedAsync(school.Id, school.Title,
                @class.Id, @class.Title,
                currentUserProvider.User.AsUser());
            if (teacher != null)
            {
                await auditStorage.LogTeacherAddedToClassAsync(school.Id, school.Title,
                    teacher.Id, teacher.FullName,
                    @class.Id, @class.Title,
                    currentUserProvider.User.AsUser());
            }

            return RedirectToAction("Details", new { schoolId, id = classId });
        }
    }
}
