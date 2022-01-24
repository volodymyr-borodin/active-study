using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ActiveStudy.Web.Areas.Crm.Models.Teachers;

public record CreateTeacherInputModel([Required] string FirstName,
    [Required] string LastName,
    string MiddleName,
    [Required, EmailAddress] string Email,
    IEnumerable<string> SubjectIds);
