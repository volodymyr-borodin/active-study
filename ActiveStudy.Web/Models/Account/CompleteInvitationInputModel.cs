using System.ComponentModel.DataAnnotations;

namespace ActiveStudy.Web.Models.Account
{
    public class CompleteInvitationInputModel
    {
        [Required]
        public string UserId { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }
    }
}