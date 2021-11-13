using ActiveStudy.Domain.Crm;
using ActiveStudy.Domain.Crm.Schools;

namespace ActiveStudy.Web.Models.Students
{
    public class CreateStudentViewModel : CreateStudentInputModel
    {
        public CreateStudentViewModel(School school, ClassShortInfo @class)
        {
            School = school;
            Class = @class;
            ClassId = @class?.Id;
        }

        public School School { get; }
        public ClassShortInfo Class { get; }
    }
}