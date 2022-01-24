using System.ComponentModel.DataAnnotations;

namespace ActiveStudy.Web.Areas.EducationMaterials.Models.TestWorks;

public record SubmitTestAuthorViewModel(
    [Required] string FirstName,
    [Required] string LastName,
    [Required] string Email);
