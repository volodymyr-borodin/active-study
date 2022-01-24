using System.ComponentModel.DataAnnotations;

namespace ActiveStudy.Web.Areas.EducationMaterials.Models.FlashCards;

public record FlashCardInputModel(
    [Required] string Term,
    [Required] string Definition);
