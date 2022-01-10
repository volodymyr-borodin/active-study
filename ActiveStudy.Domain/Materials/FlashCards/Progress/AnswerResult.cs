using System;

namespace ActiveStudy.Domain.Materials.FlashCards.Progress;

public record AnswerResult(NewAnswer Answer, FlashCard Card)
{
    public bool IsCorrect => Card.Term.Equals(Answer.Answer.Trim(), StringComparison.InvariantCultureIgnoreCase);
}
