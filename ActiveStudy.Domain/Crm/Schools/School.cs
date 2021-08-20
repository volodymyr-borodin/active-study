namespace ActiveStudy.Domain.Crm.Schools
{
    public class School
    {
        public string Id { get; }
        public string Title { get; }
        public Country Country { get; }

        public User Owner { get; }

        public School(string id, string title, Country country, User owner)
        {
            Id = id;
            Title = title;
            Country = country;
            Owner = owner;
        }
    }
}