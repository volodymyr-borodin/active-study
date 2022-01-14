using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ActiveStudy.Web.Models.FlashCards;

public class FlashCardSetCreateInputModel
{
    [Required]
    public string Title { get; set; }

    [Required]
    public List<FlashCardInputModel> Cards { get; set; }
}
