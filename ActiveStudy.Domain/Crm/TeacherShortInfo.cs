namespace ActiveStudy.Domain.Crm
{
    public class TeacherShortInfo
    {
        public string Id { get; }
        public string FullName { get; }
        public string UserId { get; }

        public TeacherShortInfo(string id, string fullName, string userId)
        {
            Id = id;
            FullName = fullName;
            UserId = userId;
        }
    }
}