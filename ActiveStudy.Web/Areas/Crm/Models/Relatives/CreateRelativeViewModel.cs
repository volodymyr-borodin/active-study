using ActiveStudy.Domain.Crm.Students;

namespace ActiveStudy.Web.Areas.Crm.Models.Relatives;

public record CreateRelativeViewModel(
    string SchoolId,
    Student Student,
    string FirstName,
    string LastName,
    string Email,
    string Phone,
    string StudentId) : CreateRelativeInputModel(
    FirstName,
    LastName,
    Email,
    Phone,
    StudentId);
