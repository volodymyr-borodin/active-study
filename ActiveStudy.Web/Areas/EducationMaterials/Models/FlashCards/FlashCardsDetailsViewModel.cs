using System.Collections.Generic;
using ActiveStudy.Domain.Materials.FlashCards;
using ActiveStudy.Domain.Materials.FlashCards.Progress;

namespace ActiveStudy.Web.Areas.EducationMaterials.Models.FlashCards;

public record FlashCardsDetailsViewModel(
    FlashCardSetDetails Set,
    IEnumerable<CardLearningProgress> CardsProgress);
