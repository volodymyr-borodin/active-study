using System;
using ActiveStudy.Domain.Crm.Scheduler;
using Microsoft.AspNetCore.Mvc;

namespace ActiveStudy.Web.Components.Scheduler
{
    public class Scheduler : ViewComponent
    {
        public IViewComponentResult Invoke(Schedule schedule,
            string schoolId,
            string classId = null,
            string teacherId = null)
        {
            return View(new SchedulerModel
            {
                Schedule = schedule,
                SchoolId = schoolId,
                ClassId = classId,
                TeacherId = teacherId,
                Lessons = new[]
                {
                    new Lesson(TimeOnly.Parse("08:30"), TimeOnly.Parse("09:15")),
                    new Lesson(TimeOnly.Parse("09:35"), TimeOnly.Parse("10:20")),
                    new Lesson(TimeOnly.Parse("10:40"), TimeOnly.Parse("11:25")),
                    new Lesson(TimeOnly.Parse("11:45"), TimeOnly.Parse("12:30")),
                    new Lesson(TimeOnly.Parse("12:50"), TimeOnly.Parse("13:35"))
                }
            });
        }
    }
}