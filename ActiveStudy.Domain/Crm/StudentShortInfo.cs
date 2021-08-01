namespace ActiveStudy.Domain.Crm
{
    public class StudentShortInfo
    {
        public string Id { get; }
        public string FullName { get; }

        public StudentShortInfo(string id, string fullName)
        {
            Id = id;
            FullName = fullName;
        }
    }
}