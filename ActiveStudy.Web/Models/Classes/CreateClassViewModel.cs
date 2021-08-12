using System.Collections.Generic;
using System.Linq;
using ActiveStudy.Domain.Crm.Teachers;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace ActiveStudy.Web.Models.Classes
{
    public class CreateClassViewModel : CreateClassInputModel
    {
        public IEnumerable<SelectListItem> Grades => new[] { new SelectListItem(null, string.Empty) }.Concat(Enumerable.Range(1, 12).Select(i => new SelectListItem(i.ToString(), i.ToString())));

        public IEnumerable<SelectListItem> Teachers { get; }

        public CreateClassViewModel(IEnumerable<Teacher> teachers)
        {
            Teachers = teachers
                .Select(s => new SelectListItem(s.FullName, s.Id))
                .Append(new SelectListItem("", null, true))
                .ToList();
        }
    }
}