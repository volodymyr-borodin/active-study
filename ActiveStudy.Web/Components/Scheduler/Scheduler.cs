using System;
using System.Linq;
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
                Periods = schedule.Periods
            });
        }
    }
}