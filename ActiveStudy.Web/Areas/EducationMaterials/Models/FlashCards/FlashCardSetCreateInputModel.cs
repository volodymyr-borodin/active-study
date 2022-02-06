using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using ActiveStudy.Domain.Materials.FlashCards;

namespace ActiveStudy.Web.Areas.EducationMaterials.Models.FlashCards;

public record FlashCardSetCreateInputModel(
    [Required] string Title,
    string Description,
    bool Public,
    [Required] List<FlashCardInputModel> Cards);
