using System.Collections.Generic;
using System.Linq;
using ActiveStudy.Domain;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace ActiveStudy.Web.Models.Teachers
{
    public class CreateTeacherViewModel : CreateTeacherInputModel
    {
        public CreateTeacherViewModel(IEnumerable<Subject> subjects)
        {
            Subjects = subjects.Select(s => new SelectListItem(s.Title, s.Id));
        }

        public IEnumerable<SelectListItem> Subjects { get; }
    }
}
