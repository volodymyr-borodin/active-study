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
                ClassId = classId
            });
        }
    }
}