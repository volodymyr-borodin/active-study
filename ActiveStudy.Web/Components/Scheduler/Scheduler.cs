using System;
using ActiveStudy.Domain.Crm.Scheduler;
using Microsoft.AspNetCore.Mvc;

namespace ActiveStudy.Web.Components.Scheduler
{
    public class Scheduler : ViewComponent
    {
        public IViewComponentResult Invoke(Schedule schedule,
            string schoolId,
            string classId)
        {
            return View(new SchedulerModel
            {
                Schedule = schedule,
                SchoolId = schoolId,
                ClassId = classId,
                Lessons = new[]
                {
                    new Lesson(TimeSpan.Parse("08:30"), TimeSpan.Parse("09:15")),
                    new Lesson(TimeSpan.Parse("09:35"), TimeSpan.Parse("10:20")),
                    new Lesson(TimeSpan.Parse("10:40"), TimeSpan.Parse("11:25")),
                    new Lesson(TimeSpan.Parse("11:45"), TimeSpan.Parse("12:30")),
                    new Lesson(TimeSpan.Parse("12:50"), TimeSpan.Parse("13:35"))
                }
            });
        }
    }
}