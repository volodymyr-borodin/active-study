using System.Collections.Generic;

namespace ActiveStudy.Domain.Materials.FlashCards;

public record FlashCard(string Id,
    string Term,
    string Definition,
    IEnumerable<Clue> Clues);
