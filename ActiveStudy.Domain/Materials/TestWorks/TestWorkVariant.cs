using System.Collections.Generic;
using System.Linq;

namespace ActiveStudy.Domain.Materials.TestWorks;

/// <summary>
/// 
/// </summary>
/// <param name="Id"></param>
/// <param name="Questions"></param>
public record TestWorkVariant(string Id, IEnumerable<Question> Questions)
{
    public decimal CalculateScore(IEnumerable<Answer> answers) =>
        Questions.Select(question =>
            question.CalculateGainedScore(answers.Single(answer => answer.QuestionId == question.Id))).Sum();
}
