namespace ActiveStudy.Web.Areas.EducationMaterials.Models.TestWorks;

public record TestFormQuestionViewModel(string Id, string Text, string Type, TestFormOptionModel[] Options = null);
