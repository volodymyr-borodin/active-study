using System.ComponentModel.DataAnnotations;

namespace ActiveStudy.Web.Models.Classes
{
    public class CreateClassInputModel
    {
        [Required]
        public string Label { get; set; }
        public int? Grade { get; set; }
    }
}
