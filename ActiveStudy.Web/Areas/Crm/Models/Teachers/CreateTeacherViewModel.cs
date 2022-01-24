using System.Collections.Generic;
using ActiveStudy.Domain.Crm.Schools;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace ActiveStudy.Web.Areas.Crm.Models.Teachers;

public record CreateTeacherViewModel(
    School School,
    SelectList Subjects,
    string FirstName,
    string LastName,
    string MiddleName,
    string Email,
    IEnumerable<string> SubjectIds) : CreateTeacherInputModel(
    FirstName,
    LastName,
    MiddleName,
    Email,
    SubjectIds);
