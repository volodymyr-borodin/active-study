using System.Collections.Generic;
using ActiveStudy.Domain.Crm.Relatives;

namespace ActiveStudy.Web.Areas.Crm.Models.Students;

public record StudentDetailsPageModel(
    string FullName,
    IEnumerable<Relative> Relatives);
