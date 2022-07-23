using System.Threading.Tasks;
using ActiveStudy.Domain.Crm.Scheduler;
using Microsoft.AspNetCore.Mvc;

namespace ActiveStudy.Web.Components.SchoolClassesSchedule;

public class SchoolClassesSchedule : ViewComponent
{
    private readonly ISchedulerStorage schedulerStorage;

    public SchoolClassesSchedule(ISchedulerStorage schedulerStorage)
    {
        this.schedulerStorage = schedulerStorage;
    }
    
    public async Task<IViewComponentResult> InvokeAsync(string schoolId)
    {
        var schoolClasses = await schedulerStorage.GetSchoolClassScheduleAsync(schoolId);
        return View(schoolClasses);
    }
}