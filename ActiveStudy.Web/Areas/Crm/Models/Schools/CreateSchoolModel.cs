using Microsoft.AspNetCore.Mvc.Rendering;

namespace ActiveStudy.Web.Areas.Crm.Models.Schools;

public record CreateSchoolModel(
    SelectList Countries,
    string Title,
    string Description,
    string CountryCode) : CreateSchoolInputModel(Title, Description, CountryCode);
