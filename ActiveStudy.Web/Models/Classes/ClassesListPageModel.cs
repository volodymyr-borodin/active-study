using System.Collections.Generic;
using ActiveStudy.Domain.Crm.Classes;
using ActiveStudy.Domain.Crm.Schools;
using ActiveStudy.Web.Models.Shared;

namespace ActiveStudy.Web.Models.Classes
{
    public class ClassesListPageModel
    {
        public ClassesListPageModel(School school,
            IEnumerable<Class> classes)
        {
            School = school;
            Classes = classes;
        }

        public School School { get; }
        public IEnumerable<Class> Classes { get; }

        public MainInfoModel MainInfo => new MainInfoModel(School.Title, "fa-university");
    }
}