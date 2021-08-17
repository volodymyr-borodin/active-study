using System;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using ActiveStudy.Domain;
using ActiveStudy.Domain.Crm;
using ActiveStudy.Domain.Crm.Classes;
using ActiveStudy.Domain.Crm.Scheduler;
using ActiveStudy.Domain.Crm.Schools;
using ActiveStudy.Domain.Crm.Teachers;
using ActiveStudy.Web.Models.Scheduler;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ActiveStudy.Web.Controllers
{
    [Route("school/{schoolId}/scheduler"), Authorize]
    public class SchedulerController : Controller
    {
        private readonly ISchedulerStorage schedulerStorage;
        private readonly ISchoolStorage schoolStorage;
        private readonly ISubjectStorage subjectStorage;
        private readonly ITeacherStorage teacherStorage;
        private readonly IClassStorage classStorage;

        public SchedulerController(ISchedulerStorage schedulerStorage,
            ISchoolStorage schoolStorage,
            ISubjectStorage subjectStorage,
            ITeacherStorage teacherStorage,
            IClassStorage classStorage)
        {
            this.schedulerStorage = schedulerStorage;
            this.schoolStorage = schoolStorage;
            this.subjectStorage = subjectStorage;
            this.teacherStorage = teacherStorage;
            this.classStorage = classStorage;
        }

        [HttpGet("create")]
        public async Task<IActionResult> Create([Required]string schoolId,
            [FromQuery]DateTime? day,
            [FromQuery]string classId)
        {
            var model = await Build(schoolId);

            if (day.HasValue)
            {
                model.Date = day.Value;
            }

            if (!string.IsNullOrEmpty(classId))
            {
                model.ClassId = classId;
            }
            
            return View(model);
        }

        [HttpPost("create")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Required]string schoolId, CreateEventInputModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(await Build(schoolId, model));
            }

            var teacher = await teacherStorage.GetByIdAsync(model.TeacherId);
            var subject = await subjectStorage.GetByIdAsync(model.SubjectId);
            var @class = await classStorage.GetByIdAsync(model.ClassId);

            var @event = new Event(string.Empty, model.SchoolId, model.Description,
                new TeacherShortInfo(teacher.Id, teacher.FullName, teacher.UserId), subject,
                new ClassShortInfo(@class.Id, @class.Title),
                new DateTime(model.Date.Date.Ticks, DateTimeKind.Utc), model.From, model.To);
            await schedulerStorage.CreateAsync(@event);

            return RedirectToAction("Details", "Classes", new { schoolId, id = @class.Id });
        }

        private async Task<CreateEventPageModel> Build(string schoolId, CreateEventInputModel input = null)
        {
            var school = await schoolStorage.GetByIdAsync(schoolId);
            var subjects = await subjectStorage.SearchAsync(school.Country.Code);
            var teachers = await teacherStorage.FindAsync(schoolId);
            var classes = await classStorage.FindAsync(schoolId);

            return new CreateEventPageModel(school, subjects, teachers, classes)
            {
                SchoolId = input?.SchoolId,
                SubjectId = input?.SubjectId,
                TeacherId = input?.TeacherId,
                ClassId = input?.ClassId,
                Description = input?.Description,
                Date = input?.Date ?? DateTime.Today,
                From = input?.From ?? TimeSpan.Zero,
                To = input?.To ?? TimeSpan.Zero
            };
        }
    }
}