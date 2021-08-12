using ActiveStudy.Domain.Crm.Students;

namespace ActiveStudy.Web.Models.Relatives
{
    public class CreateRelativeViewModel : CreateRelativeInputModel
    {
        public CreateRelativeViewModel(string schoolId, Student student)
        {
            Student = student;
            StudentId = Student?.Id;
            SchoolId = schoolId;
        }

        public Student Student { get; }
        public string SchoolId { get; }
    }
}