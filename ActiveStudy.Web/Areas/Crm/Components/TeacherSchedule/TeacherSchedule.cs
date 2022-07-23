using System.Threading.Tasks;
using ActiveStudy.Domain.Crm;
using ActiveStudy.Domain.Crm.Classes;
using ActiveStudy.Domain.Crm.Scheduler;
using ActiveStudy.Domain.Crm.Teachers;
using Microsoft.AspNetCore.Mvc;

namespace ActiveStudy.Web.Areas.Crm.Components.ClassSchedule;

public class TeacherSchedule : ViewComponent
{
    private readonly ISchedulerStorage schedulerStorage;
    private readonly ITeacherStorage teacherStorage;

    public TeacherSchedule(ISchedulerStorage schedulerStorage,
        ITeacherStorage teacherStorage)
    {
        this.schedulerStorage = schedulerStorage;
        this.teacherStorage = teacherStorage;
    }
    
    public async Task<IViewComponentResult> InvokeAsync(string schoolId, string teacherId)
    {
        var teacher = await teacherStorage.GetByIdAsync(teacherId);
        var teacherSchedule = await schedulerStorage.GetTeacherScheduleAsync(schoolId, (TeacherShortInfo)teacher);

        return View(teacherSchedule);
    }
}