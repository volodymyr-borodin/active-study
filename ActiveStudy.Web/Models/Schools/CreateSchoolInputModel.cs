using System.ComponentModel.DataAnnotations;

namespace ActiveStudy.Web.Models.Schools
{
    public class CreateSchoolInputModel
    {
        [Required]
        public string Title { get; set; }
        
        [Required]
        public string CountryCode { get; set; }
    }
}