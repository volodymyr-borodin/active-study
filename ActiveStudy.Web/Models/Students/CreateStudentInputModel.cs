using System.ComponentModel.DataAnnotations;

namespace ActiveStudy.Web.Models.Students
{
    public class CreateStudentInputModel
    {
        [Required]
        public string FirstName { get; set; }
        [Required]
        public string LastName { get; set; }
        [Required, EmailAddress]
        public string Email { get; set; }
        public string ClassId { get; set; }
    }
}