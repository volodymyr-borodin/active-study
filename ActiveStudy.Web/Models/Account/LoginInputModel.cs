using System.ComponentModel.DataAnnotations;

namespace ActiveStudy.Web.Models.Account
{
    public class LoginInputModel
    {
        [Required]
        [DataType(DataType.EmailAddress)]
        public string Username { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        public bool RememberLogin { get; set; }
    }
}