namespace ActiveStudy.Domain
{
    public class User
    {
        public string Id { get; }
        public string FullName { get; }

        public User(string id, string fullName)
        {
            Id = id;
            FullName = fullName;
        }
    }
}