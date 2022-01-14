using System.ComponentModel.DataAnnotations;

namespace ActiveStudy.Web.Models.FlashCards;

public class FlashCardInputModel
{
    [Required]
    public string Term { get; set; }

    [Required]
    public string Definition { get; set; }
}