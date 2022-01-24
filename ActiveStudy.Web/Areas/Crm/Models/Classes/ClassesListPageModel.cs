using System.Collections.Generic;
using ActiveStudy.Domain.Crm.Classes;
using ActiveStudy.Domain.Crm.Schools;

namespace ActiveStudy.Web.Areas.Crm.Models.Classes;

public record ClassesListPageModel(
    School School,
    IEnumerable<Class> Classes);
