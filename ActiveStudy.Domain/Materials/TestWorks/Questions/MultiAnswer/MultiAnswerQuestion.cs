using System;
using System.Collections.Generic;
using System.Linq;

namespace ActiveStudy.Domain.Materials.TestWorks.Questions.MultiAnswer;

public record MultiAnswerQuestion(string Id,
    string Text,
    string Description,
    decimal MaxScore,
    IEnumerable<MultiAnswerOption> Options) : Question(Id, Text, Description, MaxScore)
{
    public override decimal CalculateGainedScore(Answer answer) => answer switch
    { 
        MultiAnswer multiAnswer => CorrectAnswersCount(multiAnswer) * MaxScore / Options.Count(o => o.IsCorrect),
        _ => throw new ArgumentOutOfRangeException(nameof(answer))
    };

    private int CorrectAnswersCount(MultiAnswer multiAnswer) =>
        Options.Where(o => o.IsCorrect).Select(o => o.Id)
            .Intersect(multiAnswer.AnswerIds).Count();
}
