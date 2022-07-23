using ActiveStudy.Domain.Crm.Scheduler;
using ActiveStudy.Domain.Crm.Schools;

namespace ActiveStudy.Web.Areas.Crm.Models.Schools;

public record SchoolHomePageModel(
    School School,
    SchoolClassesSchedule Schedule);
