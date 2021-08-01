namespace ActiveStudy.Web.Models.Schools
{
    public class StudentListModel
    {
        public StudentListModel(string id, string fullName, string schoolId)
        {
            Id = id;
            FullName = fullName;
            SchoolId = schoolId;
        }

        public string Id { get; }
        public string FullName { get; }
        public string SchoolId { get; }
    }
}