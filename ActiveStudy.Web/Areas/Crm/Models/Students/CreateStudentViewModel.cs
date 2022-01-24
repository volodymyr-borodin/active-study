using ActiveStudy.Domain.Crm;
using ActiveStudy.Domain.Crm.Schools;

namespace ActiveStudy.Web.Areas.Crm.Models.Students;

public record CreateStudentViewModel(
    School School,
    ClassShortInfo Class,
    string FirstName,
    string LastName,
    string Email,
    string Phone,
    string ClassId) : CreateStudentInputModel(
    FirstName,
    LastName,
    Email,
    Phone,
    ClassId);
