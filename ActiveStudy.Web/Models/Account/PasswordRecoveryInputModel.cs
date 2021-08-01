using System.ComponentModel.DataAnnotations;

namespace ActiveStudy.Web.Models.Account
{
    public class PasswordRecoveryInputModel
    {
        [Required]
        public string UserId { get; set; }
        [Required]
        public string Code { get; set; }
        [Required]
        public string NewPassword { get; set; }
    }
}