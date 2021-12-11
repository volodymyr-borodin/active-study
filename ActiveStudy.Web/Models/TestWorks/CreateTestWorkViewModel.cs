using System.Collections.Generic;
using System.Linq;
using ActiveStudy.Domain.Materials.TestWorks;
using ActiveStudy.Domain.Materials.TestWorks.Questions.SingleAnswer;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace ActiveStudy.Web.Models.TestWorks;

public record TestWorkOption(string Text, bool IsCorrect);

public record TestWorkQuestion(string Text, string Type, List<TestWorkOption> Options, int CorrectOptionIndex);

public record TestWorkVariantInputModel(List<TestWorkQuestion> Questions);

public record CreateTestWorkInputModel(string Title,
    string Description,
    string SubjectId,
    TestWorkStatus Status,
    List<TestWorkVariantInputModel> Variants);

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
            new(new List<TestWorkQuestion>
            {
                new(string.Empty, nameof(SingleAnswerQuestion), EmptyOptions(4), 0),
                new(string.Empty, nameof(SingleAnswerQuestion), EmptyOptions(4), 0),
                new(string.Empty, nameof(SingleAnswerQuestion), EmptyOptions(4), 0)
            })
        }, subjects, statuses);
}