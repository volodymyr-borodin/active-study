using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using ActiveStudy.Domain;
using ActiveStudy.Domain.Crm;
using ActiveStudy.Domain.Crm.Classes;
using ActiveStudy.Domain.Crm.Classes.ScheduleTemplate;
using ActiveStudy.Domain.Crm.Relatives;
using ActiveStudy.Domain.Crm.Schools;
using ActiveStudy.Domain.Crm.Students;
using ActiveStudy.Domain.Crm.Teachers;
using ActiveStudy.Web.Models;
using ActiveStudy.Web.Models.Classes;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace ActiveStudy.Web.Controllers
{
    [Route("school/{schoolId}/classes"), Authorize]
    public class ClassesController : Controller
    {
        private readonly IClassStorage classStorage;
        private readonly ISchoolStorage schoolStorage;
        private readonly IStudentStorage studentStorage;
        private readonly IRelativesStorage relativesStorage;
        private readonly ITeacherStorage teacherStorage;
        private readonly ISubjectStorage subjectStorage;
        private readonly ClassManager classManager;
        private readonly IAuditStorage auditStorage;
        private readonly CurrentUserProvider currentUserProvider;

        public ClassesController(IClassStorage classStorage,
            ISchoolStorage schoolStorage,
            IStudentStorage studentStorage,
            IRelativesStorage relativesStorage,
            ITeacherStorage teacherStorage,
            ClassManager classManager,
            IAuditStorage auditStorage,
            CurrentUserProvider currentUserProvider, ISubjectStorage subjectStorage)
        {
            this.classStorage = classStorage;
            this.studentStorage = studentStorage;
            this.relativesStorage = relativesStorage;
            this.teacherStorage = teacherStorage;
            this.classManager = classManager;
            this.auditStorage = auditStorage;
            this.currentUserProvider = currentUserProvider;
            this.subjectStorage = subjectStorage;
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

            var scheduleFrom = DateOnly.FromDateTime(DateTime.Today).NearestMonday();
            if (!string.IsNullOrEmpty(scheduleDate))
            {
                if (DateOnly.TryParse(scheduleDate, out var sFrom))
                {
                    scheduleFrom = sFrom;
                }
            }
            var scheduleTo = scheduleFrom.AddDays(7);

            var schedule = await classManager.GetScheduleAsync(id, scheduleFrom, scheduleTo);

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

        [HttpGet("{id}/schedule-templates")]
        public async Task<IActionResult> ScheduleTemplates([Required] string id)
        {
            return View();
        }

        [HttpGet("{id}/schedule-templates/create")]
        public async Task<IActionResult> CreateScheduleTemplate([Required] string id)
        {
            var @class = await classStorage.GetByIdAsync(id);
            var school = await schoolStorage.GetByIdAsync(@class.SchoolId);
            var subjects = await subjectStorage.SearchAsync(school.Country.Code);
            var teachers = await teacherStorage.FindAsync(school.Id);

            var defaultTemplate = new ClassScheduleTemplateViewModel(
                teachers.Select(teacher => (TeacherShortInfo) teacher),
                subjects,
                DateOnly.FromDateTime(DateTime.Today),
                DateOnly.FromDateTime(DateTime.Today).AddDays(7),
                new []
                {
                    new ScheduleTemplateEventPeriodInputModel(new TimeOnly(8, 30), new TimeOnly(9, 15)),
                    new ScheduleTemplateEventPeriodInputModel(new TimeOnly(9, 30), new TimeOnly(10, 15)),
                    new ScheduleTemplateEventPeriodInputModel(new TimeOnly(10, 30), new TimeOnly(11, 15)),
                    new ScheduleTemplateEventPeriodInputModel(new TimeOnly(11, 30), new TimeOnly(12, 15)),
                    new ScheduleTemplateEventPeriodInputModel(new TimeOnly(12, 30), new TimeOnly(13, 15))
                },
                Enumerable.Empty<ScheduleTemplateItemInputModel>()
            );
            
            return View(defaultTemplate);
        }

        [HttpPost("{id}/schedule-template/create")]
        public async Task<IActionResult> CreateScheduleTemplate([Required] string id,
            ClassScheduleTemplateInputModel model)
        {
            var @class = await classStorage.GetByIdAsync(id);
            var periods = model.Periods.OrderBy(p => p.Start).ToList();

            var itemsByDayOfWeek = model.Items
                .GroupBy(item => item.DayOfWeek)
                .ToDictionary(grouping => grouping.Key, grouping => (IReadOnlyCollection<ScheduleTemplateItem>)
                    grouping
                        .OrderBy(i => i.Order)
                        .Select((lesson, lessonIndex) =>
                        {
                            var (start, end) = periods[lessonIndex];
                            return new ScheduleTemplateItem(start, end, (ClassShortInfo) @class, default, default);
                        }));

            var (schedule, _) = ClassScheduleTemplate.New(model.EffectiveFrom, model.EffectiveTo, itemsByDayOfWeek);
            await classManager.SaveScheduleTemplateAsync(@class, schedule);

            return RedirectToAction("ScheduleTemplates");
        }

        [HttpGet("{id}/students")]
        public async Task<IActionResult> Students([Required] string schoolId,
            [Required] string id,
            string scheduleDate = null)
        {
            var @class = await classStorage.GetByIdAsync(id);
            var students = await studentStorage.FindAsync(StudentFilter.ByClass(id));
            var school = await schoolStorage.GetByIdAsync(@class.SchoolId);

            var scheduleFrom = DateOnly.FromDateTime(DateTime.Today).NearestMonday();
            if (!string.IsNullOrEmpty(scheduleDate))
            {
                if (DateOnly.TryParse(scheduleDate, out var sFrom))
                {
                    scheduleFrom = sFrom;
                }
            }
            var scheduleTo = scheduleFrom.AddDays(7);

            var schedule = await classManager.GetScheduleAsync(id, scheduleFrom, scheduleTo);

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
