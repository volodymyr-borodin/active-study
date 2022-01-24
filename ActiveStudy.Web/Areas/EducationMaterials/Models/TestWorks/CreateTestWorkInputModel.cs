using System.Collections.Generic;
using ActiveStudy.Domain.Materials.TestWorks;

namespace ActiveStudy.Web.Areas.EducationMaterials.Models.TestWorks;

public record CreateTestWorkInputModel(string Title,
    string Description,
    string SubjectId,
    TestWorkStatus Status,
    List<TestWorkVariantInputModel> Variants);
