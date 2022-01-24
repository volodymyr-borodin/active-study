using System.Collections.Generic;
using System.Linq;
using ActiveStudy.Domain.Materials.TestWorks;

namespace ActiveStudy.Web.Areas.EducationMaterials.Models.TestWorks;

public record TestWorksListViewModel(IEnumerable<TestWork> Items, bool IsAuthenticated)
{
    public IEnumerable<TestWorkCategoryViewModel> Categories => Items
        .GroupBy(i => i.Subject.Id)
        .Select(g => new TestWorkCategoryViewModel(g.First().Subject, g.Count()));
}