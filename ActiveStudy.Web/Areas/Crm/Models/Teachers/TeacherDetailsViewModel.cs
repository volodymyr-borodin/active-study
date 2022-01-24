using ActiveStudy.Domain.Crm.Scheduler;
using ActiveStudy.Domain.Crm.Schools;

namespace ActiveStudy.Web.Areas.Crm.Models.Teachers;

public record TeacherDetailsViewModel(
    string Id,
    string FullName,
    School School,
    Schedule Schedule);
