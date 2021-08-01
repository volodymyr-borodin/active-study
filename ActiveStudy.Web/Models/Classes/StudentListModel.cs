namespace ActiveStudy.Web.Models.Classes
{
    public class StudentListModel
    {
        public StudentListModel(string id, string fullName)
        {
            Id = id;
            FullName = fullName;
        }

        public string Id { get; }
        public string FullName { get; }
    }
}