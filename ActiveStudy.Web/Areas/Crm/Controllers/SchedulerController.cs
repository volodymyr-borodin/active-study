// using System;
// using System.ComponentModel.DataAnnotations;
// using System.Threading.Tasks;
// using ActiveStudy.Domain;
// using ActiveStudy.Domain.Crm;
// using ActiveStudy.Domain.Crm.Classes;
// using ActiveStudy.Domain.Crm.Scheduler;
// using ActiveStudy.Domain.Crm.Schools;
// using ActiveStudy.Domain.Crm.Teachers;
// using ActiveStudy.Web.Areas.Crm.Models.Scheduler;
// using Microsoft.AspNetCore.Authorization;
// using Microsoft.AspNetCore.Mvc;
// using Microsoft.AspNetCore.Mvc.Rendering;
//
// namespace ActiveStudy.Web.Areas.Crm.Controllers;
//
// [Authorize]
// [Area("Crm"), Route("school/{schoolId}/scheduler")]
// public class SchedulerController : Controller
// {
//     private readonly ISchedulerStorage schedulerStorage;
//     private readonly ISchoolStorage schoolStorage;
//     private readonly ISubjectStorage subjectStorage;
//     private readonly ITeacherStorage teacherStorage;
//     private readonly IClassStorage classStorage;
//
//     public SchedulerController(ISchedulerStorage schedulerStorage,
//         ISchoolStorage schoolStorage,
//         ISubjectStorage subjectStorage,
//         ITeacherStorage teacherStorage,
//         IClassStorage classStorage)
//     {
//         this.schedulerStorage = schedulerStorage;
//         this.schoolStorage = schoolStorage;
//         this.subjectStorage = subjectStorage;
//         this.teacherStorage = teacherStorage;
//         this.classStorage = classStorage;
//     }
//
//     [HttpGet("create")]
//     public async Task<IActionResult> Create([Required]string schoolId,
//         [FromQuery]string day,
//         [FromQuery]string time,
//         [FromQuery]string classId,
//         [FromQuery]string teacherId)
//     {
//         DateOnly? pDay = null;
//         if (DateOnly.TryParse(day, out pDay))
//         {
//             model.Date = pDay;
//             model.DateLocked = true;
//         }
//         
//         var model = await Build(schoolId, new CreateEventInputModel(schoolId,
//             null,
//             teacherId,
//             classId,
//             pDay));
//
//         var timeArray = time.Split(" - ");
//         if (timeArray.Length == 2
//             && TimeOnly.TryParse(timeArray[0], out var from)
//             && TimeOnly.TryParse(timeArray[1], out var to))
//         {
//             model.From = from;
//             model.To = to;
//             model.TimeLocked = true;
//         }
//
//         if (!string.IsNullOrEmpty(teacherId))
//         {
//             model.TeacherId = teacherId;
//         }
//
//         if (!string.IsNullOrEmpty(classId))
//         {
//             model.ClassId = classId;
//         }
//             
//         return View(model);
//     }
//
//     [HttpPost("create")]
//     [ValidateAntiForgeryToken]
//     public async Task<IActionResult> Create([Required]string schoolId, CreateEventInputModel model)
//     {
//         if (!ModelState.IsValid)
//         {
//             return View(await Build(schoolId, model));
//         }
//
//         var teacher = await teacherStorage.GetByIdAsync(model.TeacherId);
//         var subject = await subjectStorage.GetByIdAsync(model.SubjectId);
//         var @class = await classStorage.GetByIdAsync(model.ClassId);
//
//         var @event = new Event(string.Empty, model.SchoolId, model.Description,
//             new TeacherShortInfo(teacher.Id, teacher.FullName, teacher.UserId), subject,
//             new ClassShortInfo(@class.Id, @class.Title),
//             model.Date, model.From, model.To);
//         await schedulerStorage.CreateAsync(@event);
//
//         return RedirectToAction("Details", "Classes", new { schoolId, id = @class.Id });
//     }
//
//     private async Task<CreateEventPageModel> Build(string schoolId, CreateEventInputModel input = null)
//     {
//         var school = await schoolStorage.GetByIdAsync(schoolId);
//         var subjects = await subjectStorage.SearchAsync(school.Country.Code);
//         var teachers = await teacherStorage.FindAsync(schoolId);
//         var classes = await classStorage.FindAsync(schoolId);
//
//         return new CreateEventPageModel(school,
//             new SelectList(subjects, "Id", "Title"),
//             new SelectList(teachers, "Id", "FullName"),
//             new SelectList(classes, "Id", "Title"),
//             false,
//             false,
//             input?.SchoolId,
//             input?.SubjectId,
//             input?.TeacherId,
//             input?.ClassId,
//             input?.Date ?? DateOnly.FromDateTime(DateTime.Today),
//             input?.From ?? TimeOnly.MinValue,
//             input?.To ?? TimeOnly.MaxValue,
//             input?.Description);
//     }
// }