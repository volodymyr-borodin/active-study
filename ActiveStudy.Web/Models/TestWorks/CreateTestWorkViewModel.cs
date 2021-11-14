using System.Collections.Generic;
using System.Linq;
using ActiveStudy.Domain.Materials.TestWorks.Questions.SingleAnswer;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace ActiveStudy.Web.Models.TestWorks;

public record TestWorkOption(string Text);

public record TestWorkQuestion(string Text, string Type, List<TestWorkOption> Options);

public record TestWorkVariantInputModel(List<TestWorkQuestion> Questions);

public record CreateTestWorkInputModel(string Title,
    string Description,
    string SubjectId,
    List<TestWorkVariantInputModel> Variants);

public record CreateTestWorkViewModel(string Title,
    string Description,
    string SubjectId,
    List<TestWorkVariantInputModel> Variants,
    IEnumerable<SelectListItem> Subjects) : CreateTestWorkInputModel(Title, Description, SubjectId, Variants)
{
    private static List<TestWorkOption> EmptyOptions(int count) =>
        Enumerable.Range(0, count)
            .Select(i => new TestWorkOption(string.Empty))
            .ToList();

    public static CreateTestWorkViewModel Empty(IEnumerable<SelectListItem> subjects) => new CreateTestWorkViewModel(
        string.Empty, string.Empty, null, new List<TestWorkVariantInputModel>
        {
            new(new List<TestWorkQuestion>
            {
                new(string.Empty, nameof(SingleAnswerQuestion), EmptyOptions(4)),
                new(string.Empty, nameof(SingleAnswerQuestion), EmptyOptions(4)),
                new(string.Empty, nameof(SingleAnswerQuestion), EmptyOptions(4))
            })
        }, subjects);
}