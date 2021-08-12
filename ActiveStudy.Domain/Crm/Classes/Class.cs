namespace ActiveStudy.Domain.Crm.Classes
{
    public class Class
    {
        public string Id { get; }
        public int? Grade { get; }
        public string Label { get; }
        public TeacherShortInfo Teacher { get; } 
        public string SchoolId { get; }

        public string Title => Grade.HasValue ? $"{Grade}-{Label}" : Label;

        public Class(string id, int? grade, string label, TeacherShortInfo teacher, string schoolId)
        {
            Id = id;
            Grade = grade;
            Label = label;
            Teacher = teacher;
            SchoolId = schoolId;
        }
    }
}