using System;
using System.Linq;
using ActiveStudy.Domain.Materials.TestWorks;
using ActiveStudy.Domain.Materials.TestWorks.Questions.MultiAnswer;
using ActiveStudy.Domain.Materials.TestWorks.Questions.SingleAnswer;

namespace ActiveStudy.Web.Areas.EducationMaterials.Models.TestWorks;

public record TestFormViewModel(
        TestWork TestWork,
        string VariantId)
    : SubmitTestViewModel(TestWork.Id,VariantId, Array.Empty<SubmitTestAnswerViewModel>(), null)
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
