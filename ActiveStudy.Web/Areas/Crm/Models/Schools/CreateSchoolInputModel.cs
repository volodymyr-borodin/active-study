using System.ComponentModel.DataAnnotations;

namespace ActiveStudy.Web.Areas.Crm.Models.Schools;

public record CreateSchoolInputModel(
    [Required] string Title,
    string Description,
    [Required] string CountryCode);
