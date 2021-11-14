using System;
using System.Collections.Generic;

namespace ActiveStudy.Domain.Materials.TestWorks.Questions.SingleAnswer;

public record SingleAnswerQuestion(string Id,
    string Text,
    string Description,
    decimal MaxScore,
    IEnumerable<SingleAnswerOption> Options,
    string CorrectAnswerId) : Question(Id, Text, Description, MaxScore)
{
    public override decimal CalculateGainedScore(Answer answer) => answer switch
    {
        SingleAnswer singleAnswer => singleAnswer.AnswerId == CorrectAnswerId
            ? MaxScore
            : 0,
        _ => throw new ArgumentOutOfRangeException(nameof(answer))
    };
}
