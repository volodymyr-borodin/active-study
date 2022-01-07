using System.Collections.Generic;
using ActiveStudy.Domain.Materials.FlashCards;

namespace ActiveStudy.Web.Models.FlashCards;

public record FlashCardsViewModel(IEnumerable<FlashCardSet> Items);
