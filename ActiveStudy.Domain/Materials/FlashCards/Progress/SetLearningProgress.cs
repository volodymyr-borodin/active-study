using System.Collections.Generic;

namespace ActiveStudy.Domain.Materials.FlashCards.Progress;

public record SetLearningProgress(string Id,
    string Title, IEnumerable<CardLearningProgress> CardsProgress) : FlashCardSet(Id, Title);
