using System.Collections.Generic;

namespace ActiveStudy.Web.Areas.Crm.Models.Classes;

public record StudentViewModel(
    string Id,
    string FullName,
    IEnumerable<RelativeViewModel> Relatives);
