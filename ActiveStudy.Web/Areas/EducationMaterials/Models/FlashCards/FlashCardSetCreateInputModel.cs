using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ActiveStudy.Web.Areas.EducationMaterials.Models.FlashCards;

public record FlashCardSetCreateInputModel(
    [Required] string Title,
    [Required] List<FlashCardInputModel> Cards);
