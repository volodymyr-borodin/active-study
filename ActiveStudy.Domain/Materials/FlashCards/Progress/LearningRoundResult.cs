using System.Collections.Generic;

namespace ActiveStudy.Domain.Materials.FlashCards.Progress;

public record LearningRoundResult(FlashCardSetDetails Set, IEnumerable<AnswerResult> Results, bool Finished);
