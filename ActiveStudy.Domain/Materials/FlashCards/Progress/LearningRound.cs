using System;
using System.Collections.Generic;
using System.Linq;

namespace ActiveStudy.Domain.Materials.FlashCards.Progress;

public record LearningRound(FlashCardSetDetails Set, IEnumerable<LearningRoundItem> Items)
{
    private readonly Random random = new Random(DateTime.UtcNow.Millisecond);

    public IEnumerable<FlashCard> GetSelectOptions(FlashCard card)
    {
        return Set.Cards
            .Where(c => c.Id != card.Id)
            .OrderBy(c => random.Next())
            .Take(3)
            .Append(card)
            .OrderBy(c => random.Next())
            .ToList();
    }
}
