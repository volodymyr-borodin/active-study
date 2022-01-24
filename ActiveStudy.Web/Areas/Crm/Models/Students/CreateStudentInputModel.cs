using System.ComponentModel.DataAnnotations;

namespace ActiveStudy.Web.Areas.Crm.Models.Students;

public record CreateStudentInputModel([Required] string FirstName,
    [Required] string LastName,
    [EmailAddress] string Email,
    [Phone] string Phone,
    string ClassId);
