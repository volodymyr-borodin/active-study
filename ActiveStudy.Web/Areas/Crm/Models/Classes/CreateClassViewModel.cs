using System.Collections.Generic;
using System.Linq;
using ActiveStudy.Domain.Crm.Schools;
using ActiveStudy.Domain.Crm.Teachers;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace ActiveStudy.Web.Areas.Crm.Models.Classes;

public record CreateClassViewModel(
    School School,
    IEnumerable<SelectListItem> Teachers,
    string Label,
    int? Grade,
    string TeacherId) : CreateClassInputModel(Label, Grade, TeacherId)
{
    public IEnumerable<SelectListItem> Grades => new[] { new SelectListItem(null, string.Empty) }.Concat(Enumerable.Range(1, 12).Select(i => new SelectListItem(i.ToString(), i.ToString())));

    // public CreateClassViewModel(School school, IEnumerable<Teacher> teachers)
    // {
    //     School = school;
    //     Teachers = teachers
    //         .Select(s => new SelectListItem(s.FullName, s.Id))
    //         .Append(new SelectListItem("", null, true))
    //         .ToList();
    // }
}