using System.Threading.Tasks;
using ActiveStudy.Domain.Crm.Scheduler;
using Microsoft.AspNetCore.Mvc;

namespace ActiveStudy.Web.Components.SchoolClassesSchedule;

public class SchoolTeachersSchedule : ViewComponent
{
    private readonly ISchedulerStorage schedulerStorage;

    public SchoolTeachersSchedule(ISchedulerStorage schedulerStorage)
    {
        this.schedulerStorage = schedulerStorage;
    }
    
    public async Task<IViewComponentResult> InvokeAsync(string schoolId)
    {
        var schoolClasses = await schedulerStorage.GetSchoolTeacherScheduleAsync(schoolId);
        return View(schoolClasses);
    }
}