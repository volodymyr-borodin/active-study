namespace ActiveStudy.Domain.Crm.Classes
{
    public class Class
    {
        public string Id { get; }
        public int? Grade { get; }
        public string Label { get; }
        public string SchoolId { get; }

        public string Title => Grade.HasValue ? $"{Grade}-{Label}" : Label;

        public Class(string id, int? grade, string label, string schoolId)
        {
            Id = id;
            Grade = grade;
            Label = label;
            SchoolId = schoolId;
        }
    }
}