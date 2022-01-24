using System.Collections.Generic;
using ActiveStudy.Domain.Crm.Schools;
using ActiveStudy.Domain.Crm.Teachers;

namespace ActiveStudy.Web.Areas.Crm.Models.Teachers;

public record TeachersListPageModel(
    School School,
    IEnumerable<Teacher> Teachers);
