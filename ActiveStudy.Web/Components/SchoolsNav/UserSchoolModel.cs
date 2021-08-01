namespace ActiveStudy.Web.Components.SchoolsNav
{
    public class UserSchoolModel
    {
        public UserSchoolModel(string id, string title)
        {
            Id = id;
            Title = title;
        }

        public string Id { get; }
        public string Title { get; }
    }
}