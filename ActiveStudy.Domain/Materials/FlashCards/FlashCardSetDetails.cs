using System.Collections.Generic;

namespace ActiveStudy.Domain.Materials.FlashCards;

public record FlashCardSetDetails(string Id,
    string Title,
    string Description,
    IEnumerable<FlashCard> Cards);
