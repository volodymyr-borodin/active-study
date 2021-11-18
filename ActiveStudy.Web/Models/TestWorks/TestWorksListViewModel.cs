using System.Collections.Generic;
using System.Linq;
using ActiveStudy.Domain;
using ActiveStudy.Domain.Materials.TestWorks;

namespace ActiveStudy.Web.Models.TestWorks;

public record TestWorkCategoryViewModel(Subject Subject, int TestsCount);

public record TestWorksListViewModel(IEnumerable<TestWork> Items, bool IsAuthenticated)
{
    public IEnumerable<TestWorkCategoryViewModel> Categories => Items
        .GroupBy(i => i.Subject.Id)
        .Select(g => new TestWorkCategoryViewModel(g.First().Subject, g.Count()));
}

public record TestWorksCategoryViewModel(IEnumerable<TestWork> Items, Subject Subject, bool IsAuthenticated);
