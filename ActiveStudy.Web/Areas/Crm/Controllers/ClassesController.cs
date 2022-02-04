using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using ActiveStudy.Domain;
using ActiveStudy.Domain.Crm;
using ActiveStudy.Domain.Crm.Classes;
using ActiveStudy.Domain.Crm.Classes.ScheduleTemplate;
using ActiveStudy.Domain.Crm.Identity;
using ActiveStudy.Domain.Crm.Relatives;
using ActiveStudy.Domain.Crm.Schools;
using ActiveStudy.Domain.Crm.Students;
using ActiveStudy.Domain.Crm.Teachers;
using ActiveStudy.Web.Areas.Crm.Models.Classes;
using ActiveStudy.Web.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace ActiveStudy.Web.Areas.Crm.Controllers;

[Authorize]
[Area("Crm"), Route("school/{schoolId}/classes")]
public class ClassesController : Controller
{
    private readonly IClassStorage classStorage;
    private readonly ISchoolStorage schoolStorage;
    private readonly IStudentStorage studentStorage;
    private readonly IRelativesStorage relativesStorage;
    private readonly ITeacherStorage teacherStorage;
    private readonly IAccessResolver accessResolver;
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
        CurrentUserProvider currentUserProvider,
        IAccessResolver accessResolver)
    {
        this.classStorage = classStorage;
        this.studentStorage = studentStorage;
        this.relativesStorage = relativesStorage;
        this.teacherStorage = teacherStorage;
        this.classManager = classManager;
        this.auditStorage = auditStorage;
        this.currentUserProvider = currentUserProvider;
        this.accessResolver = accessResolver;
        this.schoolStorage = schoolStorage;
    }

    [HttpGet]
    public async Task<IActionResult> List([Required]string schoolId)
    {
        if (!await accessResolver.HasReadAccessAsync(User, schoolId, Sections.Classes))
        {
            return Forbid();
        }
        
        var school = await schoolStorage.GetByIdAsync(schoolId);
        var classes = await classStorage.FindAsync(schoolId);

        return View(new ClassesListPageModel(school, classes.OrderBy(c => c.Grade).ThenBy(c => c.Label)));
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> Details([Required] string schoolId,
        [Required] string id,
        string scheduleDate = null)
    {
        if (!await accessResolver.HasReadAccessAsync(User, schoolId, Sections.Classes))
        {
            return Forbid();
        }

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

    [HttpGet("{id}/schedule-templates/create")]
    public async Task<IActionResult> CreateScheduleTemplate(
        [Required] string schoolId,
        [Required] string id)
    {
        if (!await accessResolver.HasReadAccessAsync(User, schoolId, Sections.Classes))
        {
            return Forbid();
        }

        var schedule = await classStorage.GetScheduleTemplateAsync(id);
        var @class = await classStorage.GetByIdAsync(id);
        var subjects = (await schoolStorage.GetSubjectsAsync(@class.SchoolId))
            .Select(subject => new SelectListItem(subject.Title, subject.Id)).Append(new SelectListItem("", null, true));
        var teachers = (await teacherStorage.FindAsync(@class.SchoolId))
            .Select(teacher => new SelectListItem(teacher.FullName, teacher.Id)).Append(new SelectListItem("", null, true));

        if (schedule == null)
        {
            var defaultTemplate = new ClassScheduleTemplateViewModel(
                @class, teachers, subjects,
                DateOnly.FromDateTime(DateTime.Today), DateOnly.FromDateTime(DateTime.Today).AddDays(7),
                new List<ScheduleTemplateEventPeriodInputModel>
                {
                    new ScheduleTemplateEventPeriodInputModel(new TimeOnly(8, 30), new TimeOnly(9, 15), new Dictionary<DayOfWeek, ScheduleTemplateItemInputModel>
                    {
                        [DayOfWeek.Monday] = null,
                        [DayOfWeek.Tuesday] = null,
                        [DayOfWeek.Wednesday] = null,
                        [DayOfWeek.Thursday] = null,
                        [DayOfWeek.Friday] = null,
                        [DayOfWeek.Saturday] = null,
                        [DayOfWeek.Sunday] = null
                    }),
                    new ScheduleTemplateEventPeriodInputModel(new TimeOnly(9, 30), new TimeOnly(10, 15), new Dictionary<DayOfWeek, ScheduleTemplateItemInputModel>(){
                        [DayOfWeek.Monday] = null,
                        [DayOfWeek.Tuesday] = null,
                        [DayOfWeek.Wednesday] = null,
                        [DayOfWeek.Thursday] = null,
                        [DayOfWeek.Friday] = null,
                        [DayOfWeek.Saturday] = null,
                        [DayOfWeek.Sunday] = null
                    }),
                    new ScheduleTemplateEventPeriodInputModel(new TimeOnly(10, 30), new TimeOnly(11, 15), new Dictionary<DayOfWeek, ScheduleTemplateItemInputModel>(){
                        [DayOfWeek.Monday] = null,
                        [DayOfWeek.Tuesday] = null,
                        [DayOfWeek.Wednesday] = null,
                        [DayOfWeek.Thursday] = null,
                        [DayOfWeek.Friday] = null,
                        [DayOfWeek.Saturday] = null,
                        [DayOfWeek.Sunday] = null
                    }),
                    new ScheduleTemplateEventPeriodInputModel(new TimeOnly(11, 30), new TimeOnly(12, 15), new Dictionary<DayOfWeek, ScheduleTemplateItemInputModel>(){
                        [DayOfWeek.Monday] = null,
                        [DayOfWeek.Tuesday] = null,
                        [DayOfWeek.Wednesday] = null,
                        [DayOfWeek.Thursday] = null,
                        [DayOfWeek.Friday] = null,
                        [DayOfWeek.Saturday] = null,
                        [DayOfWeek.Sunday] = null
                    }),
                    new ScheduleTemplateEventPeriodInputModel(new TimeOnly(12, 30), new TimeOnly(13, 15), new Dictionary<DayOfWeek, ScheduleTemplateItemInputModel>(){
                        [DayOfWeek.Monday] = null,
                        [DayOfWeek.Tuesday] = null,
                        [DayOfWeek.Wednesday] = null,
                        [DayOfWeek.Thursday] = null,
                        [DayOfWeek.Friday] = null,
                        [DayOfWeek.Saturday] = null,
                        [DayOfWeek.Sunday] = null
                    })
                }
            );
            
            return View(defaultTemplate);
        }

        return View(new ClassScheduleTemplateViewModel(
            @class, teachers, subjects,
            schedule.EffectiveFrom, schedule.EffectiveTo,
            schedule.Periods.Select(p => new ScheduleTemplateEventPeriodInputModel(p.Start, p.End,
                p.Lessons.ToDictionary(kv => kv.Key,
                    kv => kv.Value == null ? null : new ScheduleTemplateItemInputModel(kv.Value.Subject.Id, kv.Value.Teacher.Id)))).ToList()));
    }

    [HttpPost("{id}/schedule-templates/create")]
    public async Task<IActionResult> CreateScheduleTemplate(
        [Required] string schoolId,
        [Required] string id,
        ClassScheduleTemplateInputModel model)
    {
        if (!await accessResolver.HasReadAccessAsync(User, schoolId, Sections.Classes))
        {
            return Forbid();
        }

        var @class = await classStorage.GetByIdAsync(id);
        var classInfo = (ClassShortInfo) @class;

        var schedulePeriods = new List<SchedulePeriod>();
        var teachers = await teacherStorage.FindAsync(@class.SchoolId);
        var subjects = await schoolStorage.GetSubjectsAsync(@class.SchoolId);
        foreach (var p in model.Periods)
        {
            var period = new SchedulePeriod(p.Start, p.End, new Dictionary<DayOfWeek, ScheduleTemplateLesson>());

            foreach (var (dayOfWeek, lesson) in p.Lessons)
            {
                if (lesson.SubjectId == null || lesson.TeacherId == null)
                {
                    continue;
                }

                var teacher = teachers.First(t => t.Id == lesson.TeacherId);
                var subject = subjects.First(t => t.Id == lesson.SubjectId);
                    
                period.Lessons[dayOfWeek] = new ScheduleTemplateLesson(classInfo, (TeacherShortInfo)teacher, subject);
            }
            schedulePeriods.Add(period);
        }

        var (schedule, result) = ClassScheduleTemplate.New(model.EffectiveFrom, model.EffectiveTo, schedulePeriods);
        await classManager.SaveScheduleTemplateAsync(@class, schedule);

        return RedirectToAction("Details", new {id = id, schoolId = @class.Id});
    }

    [HttpGet("{id}/students")]
    public async Task<IActionResult> Students([Required] string schoolId,
        [Required] string id,
        string scheduleDate = null)
    {
        if (!await accessResolver.HasReadAccessAsync(User, schoolId, Sections.Students))
        {
            return Forbid();
        }
        
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
        if (!await accessResolver.HasFullAccessAsync(User, schoolId, Sections.Classes))
        {
            return Forbid();
        }
        
        var school = await schoolStorage.GetByIdAsync(schoolId);
        var teachers = await teacherStorage.FindAsync(schoolId);

        return View(new CreateClassViewModel(
            school,
            teachers
                .Select(s => new SelectListItem(s.FullName, s.Id))
                .Append(new SelectListItem("", null, true))
                .ToList(),
            string.Empty,
            null,
            null));
    }

    [HttpPost("create")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(
        [Required]string schoolId,
        CreateClassInputModel model)
    {
        if (!await accessResolver.HasFullAccessAsync(User, schoolId, Sections.Classes))
        {
            return Forbid();
        }

        var school = await schoolStorage.GetByIdAsync(schoolId);
        if (!ModelState.IsValid)
        {
            var teachers = await teacherStorage.FindAsync(schoolId);

            return View(new CreateClassViewModel(
                school,
                teachers
                    .Select(s => new SelectListItem(s.FullName, s.Id))
                    .Append(new SelectListItem("", null, true))
                    .ToList(),
                model.Label,
                model.Grade,
                model.TeacherId));
        }

        var teacher = string.IsNullOrEmpty(model.TeacherId)
            ? null
            : await teacherStorage.GetByIdAsync(model.TeacherId);

        // TODO: Validate create class access to school
        var @class = new Class(string.Empty, model.Grade, model.Label, (TeacherShortInfo) teacher, schoolId);
        var classId = await classStorage.InsertAsync(@class);

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