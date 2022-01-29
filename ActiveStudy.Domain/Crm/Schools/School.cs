namespace ActiveStudy.Domain.Crm.Schools
{
    public class School
    {
        public string Id { get; }
        public string Title { get; }
        public string Description { get; }
        public Country Country { get; }

        public User Owner { get; }

        public School(string id, string title, string description, Country country, User owner)
        {
            Id = id;
            Title = title;
            Description = description;
            Country = country;
            Owner = owner;
        }
    }
}