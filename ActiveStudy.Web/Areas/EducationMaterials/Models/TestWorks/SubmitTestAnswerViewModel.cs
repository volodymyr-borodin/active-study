using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ActiveStudy.Web.Areas.EducationMaterials.Models.TestWorks;

public record SubmitTestAnswerViewModel
{
    [Required]
    public string QuestionId { get; set; }

    [Required]
    public string QuestionType { get; set; }

    // single answer
    public string OptionId { get; set; }

    // multi answer
    public IEnumerable<string> SelectedOptionIds { get; set; }

    // number answer
    public decimal Number { get; set; }

    // text answer
    public string Text { get; set; }
}