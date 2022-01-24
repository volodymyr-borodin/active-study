using System.Collections.Generic;
using ActiveStudy.Domain.Crm;
using ActiveStudy.Domain.Crm.Scheduler;
using ActiveStudy.Domain.Crm.Schools;

namespace ActiveStudy.Web.Areas.Crm.Models.Classes;

public record ClassViewModel(
    string Id,
    string Title,
    School School,
    TeacherShortInfo Teacher,
    IEnumerable<StudentViewModel> Students,
    Schedule Schedule);
