using System.ComponentModel.DataAnnotations;

namespace ActiveStudy.Web.Models.Relatives
{
    public class CreateRelativeInputModel
    {
        [Required]
        public string FirstName { get; set; }

        [Required]
        public string LastName { get; set; }

        [EmailAddress]
        public string Email { get; set; }

        [Phone]
        public string Phone { get; set; }

        public string StudentId { get; set; }
    }
}