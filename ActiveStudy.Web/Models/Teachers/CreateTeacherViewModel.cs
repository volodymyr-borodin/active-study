using System.Collections.Generic;
using System.Linq;
using ActiveStudy.Domain;
using ActiveStudy.Domain.Crm.Schools;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace ActiveStudy.Web.Models.Teachers
{
    public class CreateTeacherViewModel : CreateTeacherInputModel
    {

        public CreateTeacherViewModel(School school, IEnumerable<Subject> subjects)
        {
            School = school;
            Subjects = subjects.Select(s => new SelectListItem(s.Title, s.Id));
        }

        public School School { get; }
        public IEnumerable<SelectListItem> Subjects { get; }
    }
}
