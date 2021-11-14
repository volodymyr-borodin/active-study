using System.Collections.Generic;
using ActiveStudy.Domain.Materials.TestWorks;

namespace ActiveStudy.Web.Models.TestWorks;

public record TestWorksListViewModel(IEnumerable<TestWork> Items);
