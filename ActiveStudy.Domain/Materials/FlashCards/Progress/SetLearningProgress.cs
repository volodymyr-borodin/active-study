using System.Collections.Generic;

namespace ActiveStudy.Domain.Materials.FlashCards.Progress;

public record SetLearningProgress(FlashCardSet Set, IEnumerable<CardLearningProgress> CardsProgress);
