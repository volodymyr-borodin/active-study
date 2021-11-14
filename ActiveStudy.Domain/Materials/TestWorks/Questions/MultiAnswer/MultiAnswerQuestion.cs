using System;
using System.Collections.Generic;
using System.Linq;
using ActiveStudy.Domain.Materials.TestWorks.Questions.SingleAnswer;

namespace ActiveStudy.Domain.Materials.TestWorks.Questions.MultiAnswer;

public record MultiAnswerQuestion(string Id,
    string Text,
    string Description,
    decimal MaxScore,
    IEnumerable<SingleAnswerOption> Options,
    IEnumerable<string> CorrectAnswerIds) : Question(Id, Text, Description, MaxScore)
{
    public override decimal CalculateGainedScore(Answer answer) => answer switch
    { 
        MultiAnswer multiAnswer => CorrectAnswersCount(multiAnswer) * MaxScore / CorrectAnswerIds.Count(),
        _ => throw new ArgumentOutOfRangeException(nameof(answer))
    };

    private int CorrectAnswersCount(MultiAnswer multiAnswer) =>
        CorrectAnswerIds.Intersect(multiAnswer.AnswerIds).Count();
}
