using System.Threading.Tasks;
using ActiveStudy.Domain.Crm;
using ActiveStudy.Domain.Crm.Classes;
using ActiveStudy.Domain.Crm.Scheduler;
using Microsoft.AspNetCore.Mvc;

namespace ActiveStudy.Web.Areas.Crm.Components.ClassSchedule;

public class ClassSchedule : ViewComponent
{
    private readonly ISchedulerStorage schedulerStorage;
    private readonly IClassStorage classStorage;

    public ClassSchedule(ISchedulerStorage schedulerStorage,
        IClassStorage classStorage)
    {
        this.schedulerStorage = schedulerStorage;
        this.classStorage = classStorage;
    }
    
    public async Task<IViewComponentResult> InvokeAsync(string schoolId, string classId)
    {
        var @class = await classStorage.GetByIdAsync(classId);
        var schoolClasses = await schedulerStorage.GetClassScheduleAsync(schoolId, (ClassShortInfo)@class);

        return View(schoolClasses);
    }
}