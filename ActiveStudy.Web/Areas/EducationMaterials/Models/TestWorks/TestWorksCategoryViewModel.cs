using System.Collections.Generic;
using ActiveStudy.Domain;
using ActiveStudy.Domain.Materials.TestWorks;

namespace ActiveStudy.Web.Areas.EducationMaterials.Models.TestWorks;

public record TestWorksCategoryViewModel(IEnumerable<TestWork> Items, Subject Subject, bool IsAuthenticated);