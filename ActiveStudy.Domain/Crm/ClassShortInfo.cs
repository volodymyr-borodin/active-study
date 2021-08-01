namespace ActiveStudy.Domain.Crm
{
    public class ClassShortInfo
    {
        public string Id { get; }
        public string Title { get; }

        public ClassShortInfo(string id, string title)
        {
            Id = id;
            Title = title;
        }
    }
}