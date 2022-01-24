using System.Collections.Generic;
using ActiveStudy.Domain.Crm.Schools;

namespace ActiveStudy.Web.Areas.Crm.Models.Schools;

public record SchoolListPageModel(IEnumerable<School> Schools);
