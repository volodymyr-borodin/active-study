namespace ActiveStudy.Domain.Crm.Students
{
    public class StudentFilter
    {
        private StudentFilter(string classId = null, string schoolId = null)
        {
            ClassId = classId;
            SchoolId = schoolId;
        }

        public string ClassId { get; }
        public string SchoolId { get; }

        public static StudentFilter ByClass(string classId)
        {
            return new StudentFilter(classId: classId);
        }

        public static StudentFilter BySchool(string schoolId)
        {
            return new StudentFilter(schoolId: schoolId);
        }
    }
}