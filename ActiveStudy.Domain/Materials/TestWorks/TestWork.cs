using System.Collections.Generic;
using System.Linq;

namespace ActiveStudy.Domain.Materials.TestWorks;

public record TestWork(string Id,
    string Title,
    string Description,
    Subject Subject,
    User Author,
    IEnumerable<TestWorkVariant> Variants,
    TestWorkStatus Status);

public record TestWorkDetails(string Title,
    string Description,
    Subject Subject,
    User Author,
    IEnumerable<TestWorkVariant> Variants,
    TestWorkStatus Status)
{
    // TODO: Variant not found
    public decimal CalculateScore(string variantId, IEnumerable<Answer> answers)
        => Variants.First(variant => variant.Id == variantId)
            .CalculateScore(answers);
}
