using System.ComponentModel.DataAnnotations;

namespace ActiveStudy.Web.Models.Account
{
    public class ForgotPasswordInputModel
    {
        [Required, EmailAddress]
        public string Email { get; set; }
    }
}