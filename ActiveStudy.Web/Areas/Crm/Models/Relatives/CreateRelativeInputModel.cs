using System.ComponentModel.DataAnnotations;

namespace ActiveStudy.Web.Areas.Crm.Models.Relatives;

public record CreateRelativeInputModel(
    [Required] string FirstName,
    [Required] string LastName,
    [EmailAddress] string Email,
    [Phone] string Phone,
    string StudentId);