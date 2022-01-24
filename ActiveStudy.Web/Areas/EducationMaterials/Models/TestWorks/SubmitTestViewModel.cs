using System.ComponentModel.DataAnnotations;

namespace ActiveStudy.Web.Areas.EducationMaterials.Models.TestWorks;

public record SubmitTestViewModel([Required] string TestWorkId,
    [Required] string VariantId,
    SubmitTestAnswerViewModel[] Answers,
    [Required] SubmitTestAuthorViewModel Author);