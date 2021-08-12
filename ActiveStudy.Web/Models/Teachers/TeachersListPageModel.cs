using System.Collections.Generic;
using ActiveStudy.Domain.Crm.Schools;
using ActiveStudy.Domain.Crm.Teachers;
using ActiveStudy.Web.Models.Shared;

namespace ActiveStudy.Web.Models.Teachers
{
    public class TeachersListPageModel
    {
        public TeachersListPageModel(School school,
            IEnumerable<Teacher> teachers)
        {
            School = school;
            Teachers = teachers;
        }

        public School School { get; }
        public IEnumerable<Teacher> Teachers { get; }

        public MainInfoModel MainInfo => new MainInfoModel(School.Title, "fa-university");
    }
}