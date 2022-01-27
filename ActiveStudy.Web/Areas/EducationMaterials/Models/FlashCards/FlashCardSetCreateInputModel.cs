using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ActiveStudy.Web.Areas.EducationMaterials.Models.FlashCards;

public record FlashCardSetCreateInputModel(
    [Required] string Title,
    string Description,
    [Required] List<FlashCardInputModel> Cards);
