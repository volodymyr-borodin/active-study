using System.Collections.Generic;
using ActiveStudy.Domain.Materials.TestWorks;
using ActiveStudy.Domain.Materials.TestWorks.Results;

namespace ActiveStudy.Web.Models.MyTestWorks;

public record MyTestWorkDetailsViewModel(TestWork TestWork, IEnumerable<TestWorkResult> Results);
