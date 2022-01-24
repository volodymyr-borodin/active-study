using System.Collections.Generic;
using ActiveStudy.Domain.Materials.FlashCards.Progress;

namespace ActiveStudy.Web.Areas.EducationMaterials.Models.FlashCards;

public record LearnInput(IEnumerable<NewAnswer> Answers);
