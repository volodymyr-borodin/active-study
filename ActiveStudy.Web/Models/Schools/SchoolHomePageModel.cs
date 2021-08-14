using ActiveStudy.Domain.Crm.Schools;

namespace ActiveStudy.Web.Models.Schools
{
    public class SchoolHomePageModel
    {
        public SchoolHomePageModel(School school)
        {
            School = school;
        }

        public School School { get; }
    }
}