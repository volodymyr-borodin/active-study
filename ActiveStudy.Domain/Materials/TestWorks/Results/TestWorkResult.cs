using System;
using System.Collections.Generic;
using System.Linq;
using ActiveStudy.Domain.Materials.TestWorks.Questions.MultiAnswer;
using ActiveStudy.Domain.Materials.TestWorks.Questions.SingleAnswer;

namespace ActiveStudy.Domain.Materials.TestWorks.Results;

public record TestWorkResultAuthor(string FirstName, string LastName, string Email);

public abstract record TestWorkQuestionResult
{
    public abstract Question Question { get; }
    public abstract decimal MaxScore { get; }
    public abstract decimal Scored { get; }
}

public record TestWorkSingleQuestionResult(SingleAnswerQuestion TypedQuestion, string OptionId) : TestWorkQuestionResult
{
    public override Question Question => TypedQuestion;
    public override decimal MaxScore => TypedQuestion.MaxScore;
    public override decimal Scored => TypedQuestion.CorrectAnswerId == OptionId ? MaxScore : 0;
}

public record TestWorkMultiQuestionResult(MultiAnswerQuestion TypedQuestion, IEnumerable<string> OptionIds) : TestWorkQuestionResult
{
    public override Question Question => TypedQuestion;
    public override decimal MaxScore => TypedQuestion.MaxScore;

    public override decimal Scored =>
        TypedQuestion.Options.All(o => o.IsCorrect && OptionIds.Contains(o.Id))
        && TypedQuestion.Options.All(o => !o.IsCorrect && !OptionIds.Contains(o.Id))
            ? MaxScore
            : 0;
}

public record TestWorkResult(string TestWorkId,
    string VariantId,
    IEnumerable<TestWorkQuestionResult> Answers,
    TestWorkResultAuthor Author,
    DateTimeOffset CreateOn)
{
    public decimal TotalScore => Answers.Sum(a => a.Scored);
    public decimal MaxScore => Answers.Sum(a => a.MaxScore);
}

// public class TestWorkTextQuestionResult : TestWorkQuestionResult
// {
//     public TestWorkTextQuestionResult(TextQuestion question, string text)
//     {
//         TypedQuestion = question;
//         Text = text;
//     }
//
//     public TextQuestion TypedQuestion { get; }
//     public string Text { get; }
//
//     public override Question Question => TypedQuestion;
//     public override int MaxScore => TypedQuestion.MaxScore;
//
//     public override decimal Scored =>
//         Text.Equals(TypedQuestion.Answer, StringComparison.CurrentCultureIgnoreCase) ? MaxScore : 0;
// }

// public class TestWorkNumberQuestionResult : TestWorkQuestionResult
// {
//     public TestWorkNumberQuestionResult(NumberQuestion question, decimal number)
//     {
//         TypedQuestion = question;
//         Number = number;
//     }
//
//     public NumberQuestion TypedQuestion { get; }
//     public decimal Number { get; }
//
//     public override Question Question => TypedQuestion;
//     public override int MaxScore => TypedQuestion.MaxScore;
//     public override decimal Scored => Number == TypedQuestion.Answer ? MaxScore : 0;
// }
