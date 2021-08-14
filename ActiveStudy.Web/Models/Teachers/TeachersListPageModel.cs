using System.Collections.Generic;
using ActiveStudy.Domain.Crm.Schools;
using ActiveStudy.Domain.Crm.Teachers;

namespace ActiveStudy.Web.Models.Teachers
{
    public class TeachersListPageModel
    {
        public TeachersListPageModel(School school, IEnumerable<Teacher> teachers)
        {
            School = school;
            Teachers = teachers;
        }

        public School School { get; }
        public IEnumerable<Teacher> Teachers { get; }
    }
}