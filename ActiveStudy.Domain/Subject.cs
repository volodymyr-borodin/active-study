namespace ActiveStudy.Domain
{
    public class Subject
    {
        public string Id { get; }
        public string Title { get; }

        public Subject(string id, string title)
        {
            Id = id;
            Title = title;
        }
    }
}