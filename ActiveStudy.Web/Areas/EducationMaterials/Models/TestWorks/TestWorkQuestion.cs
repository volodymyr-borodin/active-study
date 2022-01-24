using System.Collections.Generic;

namespace ActiveStudy.Web.Areas.EducationMaterials.Models.TestWorks;

public record TestWorkQuestion(
    string Text,
    string Type,
    List<TestWorkOption> Options,
    int CorrectOptionIndex);
