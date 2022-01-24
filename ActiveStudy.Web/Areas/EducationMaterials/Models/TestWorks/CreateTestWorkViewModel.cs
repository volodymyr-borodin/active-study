using System.Collections.Generic;
using System.Linq;
using ActiveStudy.Domain.Materials.TestWorks;
using ActiveStudy.Domain.Materials.TestWorks.Questions.SingleAnswer;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace ActiveStudy.Web.Areas.EducationMaterials.Models.TestWorks;

public record CreateTestWorkViewModel(string Title,
    string Description,
    string SubjectId,
    TestWorkStatus Status,
    List<TestWorkVariantInputModel> Variants,
    IEnumerable<SelectListItem> Subjects,
    IEnumerable<SelectListItem> Statuses) : CreateTestWorkInputModel(Title, Description, SubjectId, Status, Variants)
{
    private static List<TestWorkOption> EmptyOptions(int count) =>
        Enumerable.Range(0, count)
            .Select(i => new TestWorkOption(string.Empty, false))
            .ToList();

    public static CreateTestWorkViewModel Empty(IEnumerable<SelectListItem> subjects, IEnumerable<SelectListItem> statuses) => new CreateTestWorkViewModel(
        string.Empty, string.Empty, null, TestWorkStatus.Draft, new List<TestWorkVariantInputModel>
        {
            new TestWorkVariantInputModel(new List<TestWorkQuestion>
            {
                new TestWorkQuestion(string.Empty, nameof(SingleAnswerQuestion), EmptyOptions(4), 0),
                new TestWorkQuestion(string.Empty, nameof(SingleAnswerQuestion), EmptyOptions(4), 0),
                new TestWorkQuestion(string.Empty, nameof(SingleAnswerQuestion), EmptyOptions(4), 0)
            })
        }, subjects, statuses);
}
