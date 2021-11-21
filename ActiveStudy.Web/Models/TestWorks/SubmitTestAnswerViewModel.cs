using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using ActiveStudy.Domain.Materials.TestWorks;
using ActiveStudy.Domain.Materials.TestWorks.Questions.MultiAnswer;
using ActiveStudy.Domain.Materials.TestWorks.Questions.SingleAnswer;

namespace ActiveStudy.Web.Models.TestWorks;

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

public record SubmitTestAuthorViewModel([Required]string FirstName, string LastName, string Email);

public record SubmitTestViewModel([Required] string TestWorkId,
    [Required] string VariantId,
    SubmitTestAnswerViewModel[] Answers,
    [Required] SubmitTestAuthorViewModel Author);

public record TestAuthorViewModel(string FullName);

public record TestFormOptionModel(string Id, string Text);

public record TestFormQuestionViewModel(string Id, string Text, string Type, TestFormOptionModel[] Options = null);

public record TestFormViewModel(
        TestWork TestWork,
        string VariantId)
    : SubmitTestViewModel(TestWork.Id, VariantId, Array.Empty<SubmitTestAnswerViewModel>(), null)
{
    public TestFormQuestionViewModel[] Questions => TestWork.Variants.First(v => v.Id == VariantId)
        .Questions
        .Select(q => q switch
        {
            MultiAnswerQuestion multiSelectQuestion => new TestFormQuestionViewModel(q.Id, q.Text, nameof(MultiAnswerQuestion),
                multiSelectQuestion.Options.Select(o => new TestFormOptionModel(o.Id, o.Text)).ToArray()),
            SingleAnswerQuestion singleAnswerQuestion => new TestFormQuestionViewModel(q.Id, q.Text, nameof(SingleAnswerQuestion),
                singleAnswerQuestion.Options.Select(o => new TestFormOptionModel(o.Id, o.Text)).ToArray()),
            _ => throw new ArgumentOutOfRangeException(nameof(q), q, null)
        })
        .ToArray();
}
