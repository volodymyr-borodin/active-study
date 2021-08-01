using ActiveStudy.Domain.Crm;

namespace ActiveStudy.Web.Models.Students
{
    public class CreateStudentViewModel : CreateStudentInputModel
    {
        public CreateStudentViewModel(ClassShortInfo @class = null)
        {
            Class = @class;
            ClassId = @class?.Id;
        }

        public ClassShortInfo Class { get; }
    }
}