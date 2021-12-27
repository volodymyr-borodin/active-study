using System.ComponentModel.DataAnnotations;

namespace ActiveStudy.Web.Models.Account
{
    public class RegistrationInputModel
    {
        [Required, EmailAddress, DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        [Required]
        public string FirstName { get; set; }

        [Required]
        public string LastName { get; set; }

        [Required, DataType(DataType.Password)]
        public string Password { get; set; }
    }
}