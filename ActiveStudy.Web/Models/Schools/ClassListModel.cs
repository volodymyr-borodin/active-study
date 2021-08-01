namespace ActiveStudy.Web.Models.Schools
{
    public class ClassListModel
    {
        public ClassListModel(string id, string title, string schoolId)
        {
            Id = id;
            Title = title;
            SchoolId = schoolId;
        }

        public string Id { get; }
        public string Title { get; }
        public string SchoolId { get; }
    }
}