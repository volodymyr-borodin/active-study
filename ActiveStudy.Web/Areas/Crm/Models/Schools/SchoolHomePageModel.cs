using ActiveStudy.Domain.Crm.Schools;

namespace ActiveStudy.Web.Areas.Crm.Models.Schools;

public record SchoolHomePageModel(
    School School,
    ScheduleDisplayMode ScheduleDisplayMode);

public enum ScheduleDisplayMode
{
    ByClasses,
    ByTeachers
}